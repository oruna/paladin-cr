﻿using System.Threading.Tasks;
using System.Windows.Media;
using Paladin.Helpers;
using Paladin.Managers;
using Paladin.Settings;
using Styx;
using Styx.Common;
using Styx.WoWInternals.WoWObjects;

namespace Paladin.SpellBooks.Specs.Holy
{
    public partial class HolySpells : PaladinSpells<HolyTalents>
    {
        public async Task<bool> LayOnHandsMethod()
        {
            // TODO unify methods
            if (LayOnHands.CRSpell.Cooldown)
                return false;

            if (!Globals.Forbearance && Globals.MyHp <= PaladinSettings.Instance.HolyLoH)
                return await CastLayOnHands(StyxWoW.Me);

            if (!PaladinSettings.Instance.ProtLoHUse || !Globals.InParty)
                return false;

            // Unit.LayOnHandsTarget already does the checks for tank/healer etc
            var target = Healing.LayOnHandsTarget(PaladinSettings.Instance.HolyLoH);
            if (target == null || target.IsDead || target.IsGhost)
                return false;

            Helpers.Logger.DiagnosticLog("Lay on Hands on {0}, Distance: {1}, LOS: {2}", target.SafeName, target.Distance, target.InLineOfSpellSight);

            return await CastLayOnHands(target);
        }

        private async Task<bool> CastLayOnHands(WoWUnit unit)
        {
            if (!await LayOnHands.Cast(unit))
                return false;

            Helpers.Logger.PrintLog("Using Lay on Hands on {0} at {1}%", unit.SafeName, unit.HealthPercent);

            LastSpell = LayOnHands;
            return true;
        }
    }
}