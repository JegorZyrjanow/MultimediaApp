using Microsoft.Extensions.DependencyInjection;
using MultimediaApp.MVVM.Model;
using System.Windows;

namespace MultimediaApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            //ServiceCollection services = new ServiceCollection();
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services
                .AddSingleton<IGalleryService, GalleryService>()
                .AddTransient<MainViewModel>()
                .BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<GalleryService>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
        }
    }
}
