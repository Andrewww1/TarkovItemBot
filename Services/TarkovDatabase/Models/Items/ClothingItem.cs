﻿using Discord;
using Humanizer;
using System.Collections.Generic;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ClothingItem : CommonItem, IModifiableItem
    {
        public IReadOnlyCollection<string> Blocking { get; set; }
        public Penalties Penalties { get; set; }
        public string Type { get; set; }
        public IReadOnlyDictionary<string, Slot> Slots { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddField("Type", Type.Transform(To.TitleCase), true);

            if (Blocking.Count != 0) builder.AddField("Blocking", Blocking.Humanize(x => x.Transform(To.TitleCase)), true);

            if (Penalties.Speed != 0) builder.AddField("Speed Penalty", $"{Penalties.Speed}%", true);
            if (Penalties.Mouse != 0) builder.AddField("Turning Penalty", $"{Penalties.Mouse}%", true);
            if (Penalties.Deafness != Deafness.None) builder.AddField("Deafness", Penalties.Deafness.Humanize(), true);

            return builder;
        }
    }
}
