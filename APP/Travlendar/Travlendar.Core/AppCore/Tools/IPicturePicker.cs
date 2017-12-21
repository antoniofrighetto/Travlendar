using System.IO;
using System.Threading.Tasks;

namespace Travlendar.Core.AppCore.Tools
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync ();
    }
}
