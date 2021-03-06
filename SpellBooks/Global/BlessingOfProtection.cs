﻿using Paladin.Helpers;
using Paladin.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace Paladin.SpellBooks.Global
{
    public static class BlessingOfProtection
    {
        public static WoWPlayer Check(Spell spell, Spell divine)
        {
            if (!Styx.CommonBot.SpellManager.GlobalCooldown && spell.CRSpell.Cooldown) return null;
            if (!PaladinSettings.Instance.UseBoP) return null;

            // Unit.UnfriendlyPlayers.Any(u => u.IsMe); // TODO check if only caster are around and return

            if (!Styx.CommonBot.SpellManager.GlobalCooldown && PaladinSettings.Instance.UseBoPSelf && divine.CRSpell.CooldownTimeLeft.TotalSeconds > 15 && !StyxWoW.Me.HasForbearance() && StyxWoW.Me.HealthPercent < PaladinSettings.Instance.UseBoPHp)
                return StyxWoW.Me;

            if (divine.CRSpell.Cooldown && StyxWoW.Me.HasAura(122470) && !StyxWoW.Me.HasForbearance())
                return StyxWoW.Me;

            if (Globals.HasFocus && Globals.FocusedUnit.ToPlayer() != null && Globals.FocusedUnit.HealthPercent < PaladinSettings.Instance.UseBoPHp && Globals.FocusedUnit.Distance <= 40)
                return Globals.FocusedUnit.ToPlayer();

            if (!Globals.Arena) return null;
            
            var target = Managers.Healing.HandOfProtectionTarget(PaladinSettings.Instance.UseBoPHp);
            return target;
        }
    }
}
