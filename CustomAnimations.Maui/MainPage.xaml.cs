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
            await AnimateDeck();
        }

        private List<Point> GetPositions()
        {
            var pixelWidth = DeviceDisplay.Current.MainDisplayInfo.Width;
            var pixelHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
            var density = DeviceDisplay.Current.MainDisplayInfo.Density;

            var screenWidth = pixelWidth / density;
            var screenHeight = pixelHeight / density;

            var cardWidth = 200d;
            var cardHeight = 282.99d;

            var overlapWidth = cardWidth / 2; //100
            var overlapHeight = cardHeight / 2; //141.5

            var initialPositionX = -screenWidth / 2;
            var initialPositionY = -screenHeight / 2;

            var finalPositionX = screenWidth / 2 + overlapWidth;
            var finalPositionY = screenHeight / 2 + overlapHeight;

            var positions = new List<Point>();

            for (double x = initialPositionX; x <= finalPositionX; x += overlapWidth)
            {
                for (double y = initialPositionY; y <= finalPositionY; y += overlapHeight)
                {
                    positions.Add(new Point(x, y));
                }
            }

            return positions;
        }

        private async Task AnimateDeck()
        {
            var positions = GetPositions();

            grid.Clear();

            for (int i = 0; i < positions.Count; i++)
            {
                var image = new Image()
                {
                    Opacity = 0,
                    Source = "card.png",
                    WidthRequest = 200
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += TapGesture_Tapped;
                image.GestureRecognizers.Add(tapGesture);


                grid.Add(image);
            }

            foreach (Image card in grid.Children.TakeLast(10))
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

            foreach (Image card in grid.Children.Take(grid.Children.Count - 10))
            {
                card.Opacity = 1;
            }

            await Task.Delay(500);

            positions = positions.OrderBy(p => new Random().Next()).ToList();

            for (int i = 0; i < positions.Count; i++)
            {
                var card = (Image)grid.Children[i];

                var parentAnimation = new Animation();

                var rotation = Convert.ToBoolean(new Random().Next(2)) ? new Random().Next(325, 395) : new Random().Next(-395, -325);

                var translationXAnimation = new Animation(t => card.TranslationX = t, 0, positions[i].X);
                var translationYAnimation = new Animation(t => card.TranslationY = t, 0, positions[i].Y);
                var rotationAnimation = new Animation(v => card.Rotation = v, card.Rotation, rotation);

                parentAnimation.Add(0, 1, translationXAnimation);
                parentAnimation.Add(0, 1, translationYAnimation);
                parentAnimation.Add(0, 1, rotationAnimation);

                parentAnimation.Commit(card, "Animation", 8, 450, Easing.SinOut);
            }
        }

        private async void TapGesture_Tapped(object sender, TappedEventArgs e)
        {
            var selectedCard = sender as Image;

            foreach (Image card in grid.Children)
            {
                if (card == selectedCard)
                {
                    var parentAnimation = new Animation();

                    var initialRotation = card.Rotation > 0 ? card.Rotation -= 360 : card.Rotation += 360;
                    var finalRotation = new Random().Next(-4, 4) + new Random().NextDouble();

                    var rotationAnimation = new Animation(v => card.Rotation = v, initialRotation, finalRotation);
                    var translationXAnimation = new Animation(t => card.TranslationX = t, card.TranslationX, 0);
                    var translationYAnimation = new Animation(t => card.TranslationY = t, card.TranslationY, 0);
                    var scaleAnimation = new Animation(v => card.Scale = v, 1, 1.7);

                    parentAnimation.Add(0, 1, rotationAnimation);
                    parentAnimation.Add(0, 1, translationXAnimation);
                    parentAnimation.Add(0, 1, translationYAnimation);
                    parentAnimation.Add(0, 1, scaleAnimation);

                    parentAnimation.Commit(card, "Animation", 8, 400, Easing.SinOut);
                }
                else
                {
                    var opacityAnimation = new Animation(v => card.Opacity = v, 1, 0);

                    opacityAnimation.Commit(card, "Animation", 8, 200, Easing.SinOut);
                }
            }

            await Task.Delay(900);

            var rotationYAnimation = new Animation(v => selectedCard.RotationY = v, 0, 90);
            rotationYAnimation.Commit(selectedCard, "Animation", 8, 250, Easing.SinOut, (c, v) =>
            {
                selectedCard.Source = "clarity.png";

                var finalRotationYAnimation = new Animation(v => selectedCard.RotationY = v, -90, 0);
                finalRotationYAnimation.Commit(selectedCard, "Animation", 8, 250, Easing.SinOut);
            });
        }
    }
}
