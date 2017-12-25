using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;

using Travlendar.Core.AppCore.Pages;
using Travlendar.iOS.Renderers;

[assembly: ExportRenderer(typeof(AppointmentCreationPage), typeof(CustomCalendarPageRenderer))]
[assembly: ExportRenderer(typeof(CalendarPage), typeof(CustomCalendarPageRenderer))]
namespace Travlendar.iOS.Renderers
{
    public class CustomCalendarPageRenderer : PageRenderer
    {
        private readonly IDictionary<UIBarButtonItem, ToolbarItem> secondaryButtonsDict = new Dictionary<UIBarButtonItem, ToolbarItem>();
        private UIToolbar toolbar;
        private List<ToolbarItem> secondaryButtons;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            var page = e.NewElement as Page;
            if (page != null)
            {
                secondaryButtons = page.ToolbarItems.Where(item => item.Order == ToolbarItemOrder.Secondary).ToList();
                secondaryButtons.ForEach(button => page.ToolbarItems.Remove(button));
            }
            base.OnElementChanged(e);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (((ContentPage)Element) == null || NavigationController == null)
            {
                return;
            }

            IList<ToolbarItem> toolbarItems = null;
            if (((Page)Element) is CalendarPage)
            {
                toolbarItems = ((CalendarPage)Element).ToolbarItems;
            }
            else
            {
                toolbarItems = ((AppointmentCreationPage)Element).ToolbarItems;
            }

            UINavigationItem navigationItem = this.NavigationController.TopViewController.NavigationItem;
            List<UIBarButtonItem> leftButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
            List<UIBarButtonItem> rightButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

            /* we handle it when OnElementChanged is invoked
            secondaryButtons = new List<ToolbarItem>(toolbarItems.Where(item => item.Order == ToolbarItemOrder.Secondary));
            foreach (var button in secondaryButtons)
            {
                toolbarItems.Remove(button);
            }*/

            /* If leftButtons.Count > 0 then buttons have already rearranged properly */
            if (leftButtons.Count == 0)
            {
                for (int i = 0; i < navigationItem.RightBarButtonItems.Length; i++)
                {
                    UIBarButtonItem newButton = null;
                    UIBarButtonItem button = navigationItem.RightBarButtonItems[i];
                    if (toolbarItems[i].Priority == 0)
                    {
                        /* Buttons that should be in the left */
                        var buttonToRemove = rightButtons.FirstOrDefault();
                        if (buttonToRemove != null)
                            rightButtons.Remove(buttonToRemove);
                        if (IsSystemButton(button, ref newButton))
                        {
                            leftButtons.Add(newButton);
                        }
                        else
                        {
                            leftButtons.Add(button);
                        }
                    }
                    else
                    {
                        if (IsSystemButton(button, ref newButton))
                        {
                            var buttonToRemove = rightButtons.FirstOrDefault();
                            if (buttonToRemove != null)
                                rightButtons.Remove(buttonToRemove);
                            rightButtons.Add(newButton);
                        }
                    }
                }
            }

            /* initial version
            for (int i = 0; i < toolbarItems.Count; i++)
            {
                UIBarButtonItem newButton = null;
                UIBarButtonItem button = navigationItem.RightBarButtonItems[i];
                if (toolbarItems[i].Priority == 0) {
                    if (IsSystemButton(button, ref newButton)) {
                        leftButtons.Add(newButton);
                    } else {
                        leftButtons.Add(button);   
                    }
                } else {
                    if (IsSystemButton(button, ref newButton)) {
                        rightButtons.Add(newButton);
                    } else {
                        rightButtons.Add(button);
                    }
                }
            }*/

            navigationItem.SetRightBarButtonItems(rightButtons.ToArray(), false);
            navigationItem.SetLeftBarButtonItems(leftButtons.ToArray(), false);


            var secondaryButtonsNumber = secondaryButtons.Count;
            if (secondaryButtonsNumber > 0)
            {
                List<UIBarButtonItem> nativeSecondaryButtons = new List<UIBarButtonItem>();
                for (int i = 0, j = 0; i < secondaryButtonsNumber; i++, j++)
                {
                    if (i % 2 != 0 || (i != 0 && (i - 1) % 2 != 0))
                    {
                        UIBarButtonItem flexSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                        nativeSecondaryButtons.Add(flexSpace);
                        secondaryButtonsDict.Add(flexSpace, null);
                        j++;
                    }
                    nativeSecondaryButtons.Add(new UIBarButtonItem(secondaryButtons[i].Text, UIBarButtonItemStyle.Plain, TouchUpInside));
                    secondaryButtonsDict.Add(nativeSecondaryButtons[j], secondaryButtons[i]);
                }

                this.NavigationController.ToolbarHidden = false;
                toolbar = new UIToolbar(CGRect.Empty) { Items = nativeSecondaryButtons.ToArray() };
                toolbar.TintColor = UIColor.FromRGB(24, 149, 132);
                NavigationController.View.Add(toolbar);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (toolbar != null)
            {
                toolbar.RemoveFromSuperview();
                this.NavigationController.SetToolbarHidden(true, animated);
                toolbar = null;
                secondaryButtonsDict.Clear();
            }
        }

        private void TouchUpInside(object sender, EventArgs e)
        {
            UIBarButtonItem caller = ((UIBarButtonItem)sender);
            if (secondaryButtonsDict[caller] != null)
            {
                var command = secondaryButtonsDict[caller].Command;
                command.Execute(secondaryButtonsDict[caller].CommandParameter);
            }
        }

        private static bool IsSystemButton(UIBarButtonItem button, ref UIBarButtonItem newButton)
        {
            UIBarButtonSystemItem item;
            switch (button.Title)
            {
                case "Add":
                    item = UIBarButtonSystemItem.Add; break;
                case "Cancel":
                    item = UIBarButtonSystemItem.Cancel; break;
                case "Save":
                    item = UIBarButtonSystemItem.Save; break;
                case "BarButton":
                    item = UIBarButtonSystemItem.Refresh; break;
                default:
                    return false;
            }

            newButton = new UIBarButtonItem(item)
            {
                Action = button.Action,
                Target = button.Target
            };

            return true;
        }
    }
}
