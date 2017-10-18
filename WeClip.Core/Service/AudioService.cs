using System.Collections.Generic;
using System.Threading.Tasks;
using WeClip.Core.Model;
using WeClip.Core.Repository;

namespace WeClip.Core.Service
{
    public  class AudioService

    {
        private static AudioRepository repo = new AudioRepository();

        public async Task<List<Audio>> GetAllAudio(string url)
        {
            return await repo.GetAudio(url);
        }

    }
}
