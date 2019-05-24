﻿using System;
using Win10BloatRemover.Operations;

namespace Win10BloatRemover.Utils
{
    public enum MenuEntry
    {
        RemoveUWPApps,
        RemoveWinDefender,
        RemoveMSEdge,
        RemoveOneDrive,
        RemoveServices,
        RemoveWindowsFeatures,
        DisableTelemetry,
        DisableCortana,
        DisableAutoUpdates,
        DisableScheduledTasks,
        DisableErrorReporting,
        DisableWindowsTipsAndFeedback,
        OpenGitHubIssue,
        Credits,
        Quit
    }

    /**
     *  MenuUtils
     *  Contains helper functions to display the menu
     */
    class MenuUtils
    {
        public static void PrintHeading()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("|    Windows 10 Bloat Remover and Tweaker   |");
            Console.WriteLine("|             for version 1809              |");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
        }

        /**
         *  Prints the menu by looping on the MenuEntry enum values
         */
        public static void PrintMenu()
        {
            Console.WriteLine("-- MENU --");
            foreach (MenuEntry entry in Enum.GetValues(typeof(MenuEntry)))
                Console.WriteLine($"{(int)entry}: {GetMenuEntryDescription(entry)}");
            Console.WriteLine();
        }

        /**
         *  Waits for user input and returns the MenuEntry corresponding to the number pressed
         *  If the number doesn't map to an existing entry, null is returned
         */
        public static MenuEntry? ProcessUserInput()
        {
            bool inputIsNumeric = int.TryParse(Console.ReadLine(), out int userInputNumber);
            if (inputIsNumeric && Enum.IsDefined(typeof(MenuEntry), userInputNumber))
                return (MenuEntry) userInputNumber;
            else
                return null;
        }

        /**
         *  Given a MenuEntry enum value, gets the description corresponding to the entry
         */
        public static string GetMenuEntryDescription(MenuEntry entry)
        {
            switch (entry)
            {
                case MenuEntry.RemoveUWPApps:
                    return "UWP apps removal";
                case MenuEntry.RemoveWinDefender:
                    return "Windows Defender removal";
                case MenuEntry.RemoveMSEdge:
                    return "Microsoft Edge removal";
                case MenuEntry.RemoveOneDrive:
                    return "OneDrive removal";
                case MenuEntry.RemoveWindowsFeatures:
                    return "Windows features removal";
                case MenuEntry.DisableTelemetry:
                    return "Telemetry disabling";
                case MenuEntry.DisableAutoUpdates:
                    return "Automatic Windows updates disabling";
                case MenuEntry.DisableCortana:
                    return "Cortana disabling";
                case MenuEntry.RemoveServices:
                    return "Miscellaneous services removal";
                case MenuEntry.DisableScheduledTasks:
                    return "Useless scheduled tasks disabling";
                case MenuEntry.DisableErrorReporting:
                    return "Windows Error Reporting disabling";
                case MenuEntry.DisableWindowsTipsAndFeedback:
                    return "Windows Tips and feedback requests disabling";
                case MenuEntry.OpenGitHubIssue:
                    return "Report an issue/Suggest a feature";
                case MenuEntry.Quit:
                    return "Exit the application";
                default:
                    return entry.ToString();
            }
        }

        public static string GetMenuEntryExplanation(MenuEntry entry)
        {
            string explanation;
            switch (entry)
            {
                case MenuEntry.RemoveUWPApps:
                    explanation = "The following groups of UWP apps will be removed:\n";
                    foreach (UWPAppGroup app in Configuration.Instance.UWPAppsToRemove)
                        explanation += $"  {app.ToString()}\n";
                    return explanation + "Some specific app-related services will also be removed " +
                            "(but backed up in case you need to restore them).";

                case MenuEntry.RemoveWinDefender:
                    return "Windows Defender menu icon will remain there, but the program won't start anymore.";

                case MenuEntry.RemoveMSEdge:
                    return "Remember to unpin Edge from your taskbar, otherwise you won't be able to do it!";

                case MenuEntry.RemoveWindowsFeatures:
                    explanation = "The following features will be removed:\n";
                    foreach (string feature in Configuration.Instance.WindowsFeaturesToRemove)
                        explanation += $"  {feature}\n";
                    return explanation;

                case MenuEntry.DisableCortana:
                    return "This won't remove Cortana (otherwise the system would break), it will only be disabled " +
                           "using Group Policy and blocked by the firewall.";

                case MenuEntry.RemoveServices:
                    explanation = "The services starting with the following names will be removed:\n";
                    foreach (string service in Configuration.Instance.ServicesToRemove)
                        explanation += $"  {service}\n";
                    return explanation + "Services will be backed up in the same folder as this program executable.";

                case MenuEntry.DisableScheduledTasks:
                    explanation = "The following scheduled tasks will be disabled:\n";
                    foreach (string task in Configuration.Instance.ScheduledTasksToDisable)
                        explanation += $"  {task}\n";
                    return explanation;

                case MenuEntry.OpenGitHubIssue:
                    return "Your browser will now open on a GitHub page where you will be able to " +
                           "open an issue to report a bug or suggest a new feature.";

                case MenuEntry.DisableAutoUpdates:
                    return "Windows and Store apps automatic updates will be disabled using Group Policies.\n" + 
                           "This method won't work on Windows 10 Home. " +
                           "On that version, disable Windows Update service using msconfig instead.";

                case MenuEntry.DisableTelemetry:
                    return "This will backup and remove several telemetry-related services and disable features that " +
                           "report data to Microsoft, including MS Compatibility Telemetry, Device Census, " +
                           "SmartScreen, Steps Recorder and Compatibility Assistant.";

                case MenuEntry.DisableErrorReporting:
                    return "Windows Error Reporting will disabled by editing Group Policies, as well as by removing " +
                           "its services (after backing them up).";

                case MenuEntry.Credits:
                    return "Developed by Fs00\n" +
                           "Official GitHub repository: github.com/Fs00/Win10BloatRemover\n" +
                           "Based on Windows 10 de-botnet guide by Federico Dossena: fdossena.com\n";

                case MenuEntry.Quit:
                    return "Are you sure?";

                default:
                    return string.Empty;
            }
        }

        public static IOperation GetOperationInstanceForMenuEntry(MenuEntry entry)
        {
            switch (entry)
            {
                case MenuEntry.RemoveUWPApps:
                    return new UWPAppRemover(Configuration.Instance.UWPAppsToRemove);
                case MenuEntry.RemoveWinDefender:
                    return new WindowsDefenderRemover();
                case MenuEntry.RemoveMSEdge:
                    return new EdgeRemover();
                case MenuEntry.RemoveOneDrive:
                    return new OneDriveRemover();
                case MenuEntry.RemoveServices:
                    return new ServiceRemover(Configuration.Instance.ServicesToRemove);
                case MenuEntry.RemoveWindowsFeatures:
                    return new FeaturesRemover(Configuration.Instance.WindowsFeaturesToRemove);
                case MenuEntry.DisableTelemetry:
                    return new TelemetryDisabler();
                case MenuEntry.DisableCortana:
                    return new CortanaDisabler();
                case MenuEntry.DisableAutoUpdates:
                    return new AutoUpdatesDisabler();
                case MenuEntry.DisableScheduledTasks:
                    return new ScheduledTasksDisabler(Configuration.Instance.ScheduledTasksToDisable);
                case MenuEntry.DisableErrorReporting:
                    return new ErrorReportingDisabler();
                case MenuEntry.DisableWindowsTipsAndFeedback:
                    return new WindowsTipsDisabler();
                case MenuEntry.OpenGitHubIssue:
                    return new BrowserOpener("https://github.com/Fs00/Win10BloatRemover/issues/new");
                default:
                    throw new NotImplementedException($"Unimplemented operation: {entry.ToString()}");
            }
        }
    }
}