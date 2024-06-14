namespace CustomAnimations.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var animation = new Animation(v => image.TranslationY = v, -250, 0);
            animation.Commit(image, "Animation",8, 1000, Easing.SinIn);
        }
    }

}
