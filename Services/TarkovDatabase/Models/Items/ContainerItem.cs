﻿using Discord;
using System.Collections.Generic;
using TarkovItemBot.Helpers;

namespace TarkovItemBot.Services.TarkovDatabase
{
    public class ContainerItem : CommonItem
    {
        public IReadOnlyCollection<ContainerGrid> Grids { get; set; }

        public override EmbedBuilder ToEmbedBuilder()
        {
            var builder = base.ToEmbedBuilder();

            builder.AddGrids(Grids);

            return builder;
        }
    }
}
