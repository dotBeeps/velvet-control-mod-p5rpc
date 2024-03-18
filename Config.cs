using System.ComponentModel;
using VelvetControl.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;

namespace VelvetControl.Configuration;

public class Config : Configurable<Config>
{
    /*
        User Properties:
            - Please put all of your configurable properties here.

        By default, configuration saves as "Config.json" in mod user config folder.
        Need more config files/classes? See Configuration.cs

        Available Attributes:
        - Category
        - DisplayName
        - Description
        - DefaultValue

        // Technically Supported but not Useful
        - Browsable
        - Localizable

        The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
    */

    [DisplayName("Twitch Channel")]
    [Description("Channel ID to listen for control events.")]
    [DefaultValue("channel")]
    public string ChannelId { get; set; } = "channel";
    
    [DisplayName("Access Token")]
    [Description("Authorization token")]
    [DefaultValue("channel")]
    public string AuthToken { get; set; } = "channel";
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
    // 
}