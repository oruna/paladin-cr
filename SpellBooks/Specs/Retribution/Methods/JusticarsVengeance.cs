﻿using System.Threading.Tasks;
using Paladin.Helpers;
using Styx;
using Paladin.Settings;

namespace Paladin.SpellBooks.Specs.Retribution
{
    public partial class RetributionSpells : PaladinSpells<RetributionTalents>
    {
        public async Task<bool> JusticarsVengeanceMethod()
        {
            if (Globals.CurrentTarget == null) return false;

            if (!StyxWoW.Me.IsFacing(Globals.CurrentTarget)) return false;

            if (!MyTalents.DivinePurpose.IsActive() && StyxWoW.Me.HealthPercent > PaladinSettings.Instance.JusticarsVengeanceLife)
                return false;

            if (!MyTalents.JusticarsVengeance.IsActive())
                return false;

            if (Globals.HasDivinePurpose)
                return await CastJusticarsVengeance();
            
            if (Globals.HolyPower < 5) return false;

            return await CastJusticarsVengeance();
        }

        public async Task<bool> CastJusticarsVengeance()
        {
            if (!await JusticarsVengeance.Cast())
                return false;

            LastSpell = JusticarsVengeance;
            return true;
        }
    }
}
