using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sessao12.Tabuleiro
{
    internal class TabuleiroException : ApplicationException
    {
        public TabuleiroException(string message) : base(message) { }
    }
}
