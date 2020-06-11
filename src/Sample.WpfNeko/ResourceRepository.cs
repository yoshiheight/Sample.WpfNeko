using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Sample.WpfNeko
{
    public class ResourceRepository
    {
        private static readonly Lazy<ResourceRepository> _instance = new Lazy<ResourceRepository>(() => new ResourceRepository());

        public static ResourceRepository Instance => _instance.Value;

        private readonly Dictionary<string, BitmapSource> _imageMap;

        private ResourceRepository()
        {
            var prefix = "Sample.WpfNeko.Images.";
            _imageMap = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(path => path.StartsWith(prefix))
                .Select(path => new { Path = path.Substring(prefix.Length), Image = LoadImage(path) })
                .ToDictionary(item => item.Path, item => item.Image);

            // 埋め込みリソースから画像ファイルを同期的に読み込む。
            static BitmapSource LoadImage(string path)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
                {
                    var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    var bmp = new WriteableBitmap(decoder.Frames[0]);
                    bmp.Freeze();
                    return bmp;
                }
            }
        }

        public BitmapSource GetImage(string image)
        {
            return _imageMap[image];
        }
    }
}
