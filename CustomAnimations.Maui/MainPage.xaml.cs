namespace CustomAnimations.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            grid.Clear();

            for (int i = 0; i < 10; i++)
            {
                grid.Add(new Image()
                {
                    Opacity = 0,
                    Source = "card.png",
                    WidthRequest = 200
                });
            }

            foreach (Image card in grid.Children)
            {
                var parentAnimation = new Animation();
                var translationYAnimation = new Animation(v => card.TranslationY = v, -250, 0);
                var opacityAnimation = new Animation(v => card.Opacity = v, 0, 1);
                var scaleAnimation = new Animation(v => card.Scale = v, 1.5, 1);
                var translationXAnimation = new Animation(v => card.TranslationX = v, new Random().Next(-250, 251), 0);
                var rotationAnimation = new Animation(v => card.Rotation = v, 0, new Random().Next(-4, 5));

                parentAnimation.Add(0, 0.5, translationYAnimation);
                parentAnimation.Add(0, 0.5, translationXAnimation);
                parentAnimation.Add(0, 0.5, opacityAnimation);
                parentAnimation.Add(0, 0.5, scaleAnimation);
                parentAnimation.Add(0.5, 1, rotationAnimation);

                parentAnimation.Commit(card, "Animation", 8, 400, Easing.SinIn);

                await Task.Delay(100);
            }
        }
    }

}
