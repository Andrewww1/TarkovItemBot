﻿using Discord;
using Discord.Commands;
using Humanizer;
using System;
using System.Linq;
using System.Threading.Tasks;
using TarkovItemBot.Preconditions;
using TarkovItemBot.Services;

namespace TarkovItemBot.Modules
{
    [Name("Utility")]
    public class UtilityModule : ItemBotModuleBase
    {
        private readonly TarkovDatabaseClient _tarkov;
        private readonly TarkovSearchClient _tarkovSearch;

        public UtilityModule(TarkovDatabaseClient tarkov, TarkovSearchClient tarkovSearch)
        {
            _tarkov = tarkov;
            _tarkovSearch = tarkovSearch;
        }

        [Command("compatible")]
        [Alias("compatibility")]
        [Summary("Check if two items are compatible.")]
        [Remarks("compatible \"m4a1\" \"psg-1 grip\"")]
        public async Task CompatibilityAsync(string baseQuery, [Remainder]string modQuery)
        {
            var baseItemResult = (await _tarkovSearch.SearchAsync($"name:{baseQuery}", 1)).FirstOrDefault();
            var modItemResult = (await _tarkovSearch.SearchAsync($"name:{modQuery}", 1)).FirstOrDefault();

            if (baseItemResult == null || modItemResult == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var baseItem = await _tarkov.GetEmbedableItemAsync(baseItemResult.Id, baseItemResult.Kind);

            if (baseItem is not ModifiableItem modifiableItem)
            {
                await ReplyAsync("The base item provided is not modifiable!");
                return;
            }

            var modItem = await _tarkov.GetEmbedableItemAsync(modItemResult.Id, modItemResult.Kind) as BaseItem;

            string slotName = null;

            foreach (var slot in modifiableItem.Slots)
            {
                if (!slot.Value.Filter.Any(x => x.Value.Contains(modItem.Id))) continue;
                slotName = slot.Key;
                break;
            }

            if (slotName == null)
            {
                await ReplyAsync($"Item `{modifiableItem.Name}` does not allow for installation of `{modItem.Name}`!");
                return;
            }

            await ReplyAsync($"Item `{modifiableItem.Name}` fits `{modItem.Name}` in slot `{slotName.Humanize(LetterCasing.Title)}`.");
        }


        [Command("tax")]
        [Alias("commission", "flea", "market")]
        [Summary("Returns the Flea Market tax for the item most closely matching the query.")]
        [Remarks("tax 500000 Red Keycard")]
        public async Task TaxAsync(int price, [Remainder][RequireLength(3, 50)] string query)
        {
            var result = (await _tarkovSearch.SearchAsync($"name:{query}", 1)).FirstOrDefault();

            if (result == null)
            {
                await ReplyAsync("No items found for query!");
                return;
            }

            var item = await _tarkov.GetEmbedableItemAsync(result.Id, result.Kind) as BaseItem;

            var offerValue = item.Price;
            var requestValue = Convert.ToDouble(price);

            var offerModifier = Math.Log10(offerValue / requestValue);
            var requestModifier = Math.Log10(requestValue / offerValue);

            if (requestValue >= offerValue)
            {
                requestModifier = Math.Pow(requestModifier, 1.08);
            }
            else
            {
                offerModifier = Math.Pow(offerModifier, 1.08);
            }

            var tax = offerValue * 0.05 * Math.Pow(4, offerModifier) + requestValue * 0.05 * Math.Pow(4, requestModifier);

            var builder = new EmbedBuilder()
            {
                Title = $"{item.Name} ({item.ShortName})",
                Color = item.Grid.Color,
                ThumbnailUrl = item.IconUrl,
                Description = item.Description
            };

            builder.AddField("Base Price", $"{item.Price:#,##0} ₽", true);
            builder.AddField("Base Tax", $"{tax:#,##0} ₽", true);
            builder.AddField("Profit", $"{price - tax:#,##0} ₽", true);

            builder.WithFooter($"{item.Kind.Humanize()} • Modified {item.Modified.Humanize()}");

            await ReplyAsync(embed: builder.Build());
        }
    }
}
