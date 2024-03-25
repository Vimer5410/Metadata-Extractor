using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Input;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System.Windows;


namespace Metadata_Extractor
{
    
    public partial class MainWindow : Window
    {
        private Avalonia.Controls.Image _photoImage;
        private TextBox _textBox;
        private bool isMoving = false;
        

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _photoImage = this.FindControl<Avalonia.Controls.Image>("PhotoImage");
            _textBox = this.FindControl<TextBox>("InfoTextBox");
        }

        public void ImageData(string imagePath)
        {
            var bitmap = new Bitmap(imagePath);
            var height = bitmap.PixelSize.Height;
            var width = bitmap.PixelSize.Width;
            var format = bitmap.Format;
            
            var directories = ImageMetadataReader.ReadMetadata(imagePath);
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    _textBox.Text += $"{tag.Name} : {tag.Description}\n";
                }
            }
        }
        
        private async void Btn_LoadImage_OnClick(object sender, RoutedEventArgs e)
        {
            _textBox.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите фото";
            openFileDialog.Filters.Add(new FileDialogFilter { Name = "Файлы изображений", Extensions = { "jpg", "png", "bmp" } });

            string[] selectedPhotos = await openFileDialog.ShowAsync(this);

            if (selectedPhotos != null && selectedPhotos.Length > 0)
            {
                Bitmap bitmap = new Bitmap(selectedPhotos[0]);
                _photoImage.Source = bitmap;
                ImageData(selectedPhotos[0]);
            }
        }

        

        //ButtonEventHandler
        private void Btn_Close_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            BeginMoveDrag(e);
        }
        private void Btn_Curtail_OnClick(object? sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Btn_Copy_OnClick(object? sender, RoutedEventArgs e)
        {
            Clipboard.SetTextAsync(_textBox.Text);
        }

        private void Btn_Clear_OnClick(object? sender, RoutedEventArgs e)
        {
            _textBox.Text = "";
        }
    }
}