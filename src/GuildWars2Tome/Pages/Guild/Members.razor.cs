﻿using GuildWars2Tome.Extensions;
using GuildWars2Tome.Models;
using JK.GuildWars2Api;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuildWars2Tome.Pages.Guild
{
    public partial class Members
    {
        private IEnumerable<Member> members = new List<Member>();

        [Inject]
        protected IGuildWars2ApiClient GuildWarsClient { get; set; }

        [Inject]
        protected IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var key = await JS.LocalStorageGet<string>(StorageKeys.SettingsApiKey);
            if (!string.IsNullOrWhiteSpace(key))
            {
                this.GuildWarsClient.Key = key;
                var guildId = await JS.LocalStorageGet<string>(StorageKeys.SettingsGuildId);
                var guildMembers = await this.GuildWarsClient.V2.GetGuildMembersAsync(guildId);
                this.members = guildMembers.Select(x => new Member(x)).OrderByDescending(x => x.Joined);
            }
        }
    }
}
