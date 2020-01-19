using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpectabisLib.Enums;

namespace SpectabisLib.Repositories
{
    public class CancellationTokenRepository
    {
        private Dictionary<CancellationTokenKey, CancellationTokenSource> _tokenContainer = new Dictionary<CancellationTokenKey,CancellationTokenSource>();

        public CancellationTokenRepository()
        {
            _tokenContainer.Add(CancellationTokenKey.SpectabisApp, new CancellationTokenSource());
        }

        public void CancelToken(CancellationTokenKey key)
        {
            _tokenContainer[key].Cancel();
        }

        public CancellationToken GetToken(CancellationTokenKey key)
        {
            return _tokenContainer[key].Token;
        }
    }
}
