using EMV.Models.Files;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models.Files
{
    public class GfxFileModel : ModFileBase
    {
        public IList<GfxModel> Gfx { get; set; }
        public IList<GFXOther> OtherGfx { get; set; }

        public GfxFileModel()
        {
            Gfx = new List<GfxModel>();
            OtherGfx = new List<GFXOther>();
        }

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == null)
                return;

            if (token.Equals("spriteType") || token.Equals("textSpriteType"))
                Gfx.Add(parser.Parse(new GfxModel()));
            else if (!token.Equals("spriteTypes"))
                OtherGfx.Add(parser.Parse(new GFXOther(token, null)));
        }

        public override void BindLocalisation(Dictionary<string, string> localisation)
        {
        }
    }
}
