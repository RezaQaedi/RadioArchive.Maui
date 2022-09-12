namespace RadioArchive.Maui.Animation;
using CommunityToolkit.Maui.Animations;


class ScaleDownAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view)
    {
        await view.ScaleTo(0.5, Length, Easing);
        await view.ScaleTo(1, Length, Easing);
    }
}

class RotateLeftAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view)
    {
        await view.RotateTo(-90, Length, Easing);
        await view.RotateTo(0, Length, Easing);
    }
}

class RotateRightAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view)
    {
        await view.RotateTo(+90, Length, Easing);
        await view.RotateTo(0, Length, Easing);
    }
}

class RotateAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view)
    {
        await view.RotateTo(360, Length, Easing);
        view.Rotation = 0;
    }
}

class FadingAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view)
    {
        await view.FadeTo(0, Length, Easing);
        await view.FadeTo(1, Length, Easing);
    }
}