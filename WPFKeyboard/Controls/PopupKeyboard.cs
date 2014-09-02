using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace WPFKeyboard.Controls
{
    public class PopupKeyboard : Popup
    {
        public PopupKeyboard()
        {
            AllowsTransparency = true;
            Placement = PlacementMode.Absolute;
        }

        public void Show(FrameworkElement focusedElement)
        {
            var allScreens = System.Windows.Forms.Screen.AllScreens;

            // Sometimes the visual is not connected to a presentation source so we return if this is the case.
            if (PresentationSource.FromVisual(focusedElement) == null)
                return;

            // get the x/y for the control based off of the entire viewing area
            var locationFromScreen = focusedElement.PointToScreen(focusedElement.TranslatePoint(new Point(0, 0), this));
            var elementRectangle = new System.Drawing.Rectangle((int)locationFromScreen.X, (int)locationFromScreen.Y,
                (int)focusedElement.ActualWidth, (int)focusedElement.ActualHeight);

            // return a rectangle representing screen available
            var screens = allScreens
                .Select(screen => new System.Drawing.Rectangle(screen.Bounds.Left, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height))
                .ToList();

            // get rectangles for the intersection of the element for each screen.
            // if the control is not within the screen, it be IsEmpty.
            // we need to use the screen with the largest intersection
            var intersections =
                screens.Select(screen => System.Drawing.Rectangle.Intersect(screen, elementRectangle))
                .Select(intersection => intersection.IsEmpty ? 0 : (intersection.Width * intersection.Height))
                .ToList();

            // our target screen is the screen with the largest intersection
            var screenIndex = intersections.IndexOf(intersections.Max());
            if (screenIndex == -1) screenIndex = 0;
            var targetScreen = allScreens[intersections.IndexOf(intersections.Max())];

            // now that we have our screen, lets split the screen in half horizontally.
            // we will then use these intersections to determine if we should render the keyboard
            // on the top or bottom
            var topHalf = new System.Drawing.Rectangle(
                targetScreen.Bounds.X,
                targetScreen.Bounds.Y,
                targetScreen.Bounds.Width,
                targetScreen.Bounds.Height / 2);
            topHalf.Intersect(elementRectangle);
            var bottomHalf = new System.Drawing.Rectangle(
                targetScreen.Bounds.X,
                targetScreen.Bounds.Y + (targetScreen.Bounds.Height / 2),
                targetScreen.Bounds.Width,
                targetScreen.Bounds.Height / 2);
            bottomHalf.Intersect(elementRectangle);

            // are we showing the keyboard on the top or bottom of the monitor?
            var isBottom = (topHalf.IsEmpty ? 0 : topHalf.X * topHalf.X) >
                            (bottomHalf.IsEmpty ? 0 : bottomHalf.X * bottomHalf.Y);

            PlacementRectangle = isBottom
                ? new Rect(targetScreen.Bounds.X + ((targetScreen.Bounds.Width - Width) / 2), targetScreen.Bounds.Y + (targetScreen.Bounds.Height - Height), Width, Height)
                : new Rect(targetScreen.Bounds.X + ((targetScreen.Bounds.Width - Width) / 2), targetScreen.Bounds.Y, Width, Height);

            IsOpen = true;
        }

        public void Hide()
        {
            IsOpen = false;
        }
    }
}
