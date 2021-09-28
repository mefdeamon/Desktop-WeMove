using Deamon.UiCore.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Deamon.Views.Sign
{
    public class AnimatableBaseView : UserControl
    {

        public AnimationDirection LoadedAnimateDirection { get; set; } = AnimationDirection.FromLeft;

        public AnimationDirection UnLoadAnimateDirection { get; set; } = AnimationDirection.ToRight;

        public bool ShouldAnimatedOut { get; set; } = false;

        public AnimatableBaseView()
        {
            Loaded += AnimatableBaseView_Loaded;
        }

        private async void AnimatableBaseView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ShouldAnimatedOut)
                await AddAnimationOut();
            else
                await AddAnimationIn();
        }

        private async Task AddAnimationIn()
        {
            await this.SlideAndFadeInAsync(LoadedAnimateDirection);
        }

        private async Task AddAnimationOut()
        {
            await this.SlideAndFadeOutAsync(UnLoadAnimateDirection);
        }
    }


    public enum AnimationDirection
    {
        None,
        FromLeft,
        FromRight,
        ToLeft,
        ToRight
    }
}
