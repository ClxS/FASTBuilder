﻿using System;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using FastBuilder.Services;

namespace FastBuilder.Views.Build
{

	public partial class BuildSessionView
	{
		private bool _isAutoScrollingContent;
		private double _previousHorizontalScrollOffset;
		
		public BuildSessionView()
		{
			InitializeComponent();
			_isAutoScrollingContent = true;
			_previousHorizontalScrollOffset = this.ContentScrollViewer.ScrollableWidth;
		}

		private void ContentScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			var horizontalOffset = e.HorizontalOffset;
			if (_isAutoScrollingContent)
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (horizontalOffset == _previousHorizontalScrollOffset)    // which means the scroll is not actually changed, but content size is changed
				{
					this.ContentScrollViewer.ScrollToHorizontalOffset(this.ContentScrollViewer.ScrollableWidth);
					horizontalOffset = this.ContentScrollViewer.ScrollableWidth;
				}
			}

			// if we are scrolled to the right end, turn on auto scrolling
			_isAutoScrollingContent = Math.Abs(horizontalOffset - this.ContentScrollViewer.ScrollableWidth) < 1;

			_previousHorizontalScrollOffset = horizontalOffset;

			if (this.ContentScrollViewer.ScrollableHeight > 0)
			{
				var offset = e.VerticalOffset * this.HeaderScrollViewer.ScrollableHeight / this.ContentScrollViewer.ScrollableHeight;
				this.HeaderScrollViewer.ScrollToVerticalOffset(offset);
				this.BackgroundScrollViewer.ScrollToVerticalOffset(offset);
			}

			this.UpdateViewTimeRange(ViewTimeRangeChangeReason.Scroll);
		}

		private void UpdateViewTimeRange(ViewTimeRangeChangeReason reason)
		{
			var viewTransformService = IoC.Get<IViewTransformService>();

			var headerViewWidth = (double)this.FindResource("HeaderViewWidth");

			var startTime = (this.ContentScrollViewer.HorizontalOffset - headerViewWidth) / viewTransformService.Scaling;
			var duration = this.ContentScrollViewer.ActualWidth / viewTransformService.Scaling;
			var endTime = startTime + duration;
			viewTransformService.SetViewTimeRange(startTime, endTime, reason);
		}

		private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			var scaleService = IoC.Get<IViewTransformService>();
			scaleService.Scaling = scaleService.Scaling * (1 + e.Delta / 1200.0);

			this.UpdateViewTimeRange(ViewTimeRangeChangeReason.Zoom);

			e.Handled = true;
		}

	}
}
