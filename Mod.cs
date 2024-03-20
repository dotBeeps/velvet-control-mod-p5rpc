using System.Text.Json;
using Newtonsoft.Json.Linq;
using p5rpc.lib.interfaces;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using VelvetControl.Template;
using VelvetControl.Configuration;

namespace VelvetControl;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{
    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    private readonly IModLoader _modLoader;

    /// <summary>
    /// Provides access to the Reloaded.Hooks API.
    /// </summary>
    /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
    private readonly IReloadedHooks? _hooks;

    /// <summary>
    /// Provides access to the Reloaded logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Entry point into the mod, instance that created this class.
    /// </summary>
    private readonly IMod _owner;

    /// <summary>
    /// Provides access to this mod's configuration.
    /// </summary>
    private Config _configuration;

    /// <summary>
    /// The configuration of the currently executing mod.
    /// </summary>
    private readonly IModConfig _modConfig;

    private readonly IP5RLib _p5rLib;
    private readonly TwitchPubSub _twitchPubSub;
    private readonly String _extensionId = "7u6rvbvy2nn0dsze7p32psunopxbj7";
    private readonly PersonaHelper _personaHelper;
    private readonly CommandHelper _commandHelper;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;
        var p5RLibController = _modLoader.GetController<IP5RLib>();
        if (p5RLibController == null || !p5RLibController.TryGetTarget(out var p5RLib))
        {
            _logger.WriteLine("Hey, problem, couldn't get my p5r inputhooks, Velvet Control won't work.");
        }
        else
        {
            _p5rLib = p5RLib;
            _personaHelper = new PersonaHelper(_p5rLib, _logger);
            _commandHelper = new CommandHelper(_personaHelper);
            _p5rLib.Sequencer.EventStarted += EventStarted;
            _p5rLib.Sequencer.SequenceChanged += SequenceChanged;
            _twitchPubSub = new TwitchPubSub();
            new Task<Task>(async () =>
            {
                string? authCode = await LoginFlow.GetTwitchAuthCode();
                if (authCode is not null)
                {
                    var token = await LoginFlow.AuthWithServer(authCode);
                    if (token is not null)
                    {
                        _twitchPubSub.OnPubSubServiceConnected += OnPubSubConnected;
                        _twitchPubSub.OnChannelExtensionBroadcast += OnBroadcast;
                        _twitchPubSub.OnListenResponse += onListenResponse;
                        _logger.WriteLine("[Velvet Control] Set up twitch hooks.");
                        _twitchPubSub.Connect();
                    }
                    else
                    {
                        _logger.WriteLine("[Velvet Control] Error retrieving auth token, please try relaunching.");
                    }
                }
                else
                {
                    _logger.WriteLine("[Velvet Control] User refused auth, disabling.");
                }
            }).Start();
            Util.Init(_logger,_configuration);
            PartyStatusHelper.Init(_logger,_configuration);
            RosterHelper.Init(_logger,_configuration);
        }


    }

    private void onListenResponse(object sender, OnListenResponseArgs e)
    {
        if (!e.Successful)
            _logger.WriteLine($"[Velvet Control] Failed to listen to extension! Response: {e.Response.Error}");
        else
        {
            _logger.WriteLine($"[Velvet Control] Connected to extension broadcast successfully.");
        }
    }
    
    private void OnPubSubConnected(object sender, EventArgs e)
    {
        _logger.WriteLine("[Velvet Control] Connected to PubSub.");
        _twitchPubSub.ListenToChannelExtensionBroadcast(_configuration.ChannelId, _extensionId);
        _twitchPubSub.SendTopics(_configuration.AuthToken);
    }
    
    private void OnBroadcast(object sender, OnChannelExtensionBroadcastArgs e)
    {
        JObject jObject = JObject.Parse(e.Messages[0]);
        VelvetControlMessage? message = jObject.ToObject<VelvetControlMessage>();
        if (message != null)
        {
            _logger.WriteLine(message.command);
            if (_commandHelper.Commands.ContainsKey(message.command))
            {
                _logger.WriteLine($"Calling: {message.command}");
                new Thread(() =>
                {
                    PartyStatusHelper.DebugParty();
                    _commandHelper.Commands[message.command].Invoke(message.parameters);
                }).Start();
            }
        }
    }
    
    private void EventStarted(Sequence.EventInfo eventInfo)
    {
        _logger.WriteLine($"Event Info: {eventInfo}");
    }

    private void SequenceChanged(Sequence.SequenceInfo sequenceInfo)
    {
        _logger.WriteLine($"Sequence Info: {sequenceInfo}");
    }

    #region Standard Overrides

    public override void ConfigurationUpdated(Config configuration)
    {
        if (_twitchPubSub != null)
        {
            _twitchPubSub.Disconnect();
        }
        // Apply settings from configuration.
        // ... your code here.
        _configuration = configuration;
        _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        if (_twitchPubSub != null)
        {
            _twitchPubSub.Connect();
        }
    }

    #endregion

    #region For Exports, Serialization etc.

#pragma warning disable CS8618 
    public Mod()
    {
    }
#pragma warning restore CS8618

    #endregion
}