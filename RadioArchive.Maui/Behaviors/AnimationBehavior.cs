using Anim = Microsoft.Maui.Controls.Animation;

namespace RadioArchive.Maui.Behaviors
{
    public class RotationBehavior : Behavior<View>
    {
        Anim _animation;

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            _animation = new Anim(v => bindable.Rotation = v, 0, 360, Easing.Linear);
            bindable.PropertyChanged += Bindable_PropertyChanged;
            // Perform setup
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            _animation.Dispose();
            bindable.PropertyChanged -= Bindable_PropertyChanged;
            // Perform clean up
        }

        private void Bindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is not VisualElement bindable)
                return;

            if (e.PropertyName == nameof(bindable.IsVisible))
            {
                if (bindable.IsVisible)
                {
                   _animation.Commit(bindable, "rotate", 16, 1000, Easing.Linear, (v, c) => bindable.Rotation = 0, () => true);
                }
                else
                {
                    bindable.AbortAnimation("rotate");
                }
            }
        }
    }

    public class FadingBehavior : Behavior<View>
    {
        Anim _animation;

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            _animation = new Anim(v => bindable.Opacity = v, 0, 1, Easing.Linear);
            bindable.PropertyChanged += Bindable_PropertyChanged;
            // Perform setup
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            _animation.Dispose();
            bindable.PropertyChanged -= Bindable_PropertyChanged;
            // Perform clean up
        }

        private void Bindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is not VisualElement bindable)
                return;

            if (e.PropertyName == nameof(bindable.IsVisible))
            {

                if (bindable.IsVisible)
                {
                    _animation.Commit(bindable, "fade", 16, 1000, Easing.Linear, (v, c) => bindable.Opacity = 0, () => true);
                    System.Diagnostics.Debug.WriteLine("starting animation!");
                }
                else
                {
                    bindable.AbortAnimation("fade");
                    System.Diagnostics.Debug.WriteLine("Aboring!");
                }
            }
        }
    }
}
