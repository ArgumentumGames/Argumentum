{
  "Form": {
    "Buttons": {
      "Save": "Save (ctrl + s)",
      "SaveAndClose": "Save and close",
      "CreateAndClose": "Create and close",
      "ActionAndCloseShortcut": "Shortcut: ctrl + enter",
      "Done": "Done",
      "Exit.Tip": "Exit - if something changed, you will be asked to save",
      "Return.Tip": "Return to previous dialog",
      "History.Tip": "History / previous versions",
      "Metadata.Tip": "This item is metadata for:",
      "Note": {
        "Add": "Add note",
        "ItemNotSaved": "To add a note, please save item first"
      }
    }
  },
  "PublishStatus": {
    "Label": "Status:",
    "show": "show",
    "show.Tip": "Changes are public",
    "hide": "hide",
    "hide.Tip": "This item is not publicly visible",
    "branch": "draft",
    "branch.Tip": "Changes are only visible to editors",
    "Dialog": {
      "Title": "Save Mode",
      "Intro": "This determines how you will save. The default is show/publish.",
      "Show": {
        "Title": "Show / Publish Everything",
        "Body": "Show changes to the public after you save."
      },
      "Hide": {
        "Title": "Hide Everything",
        "Body": "This item will be hidden and only visible to content editors."
      },
      "Branch": {
        "Title": "Draft / Hide Changes",
        "Body": "Only editors can see changes until published at a later time."
      }
    }
  },
  "Message": {
    "Saved": "Saved",
    "Saving": "Saving...",
    "Deleted": "Deleted",
    "Deleting": "Deleting...",
    "DeleteError": "Delete failed. Please check console for more information",
    "SwitchedLanguageToDefault": "We have switched language to default {{language}} because it's missing some or all values",
    "CantSwitchLanguage": "Can't switch languages until current language has all required values",
    "PastingFilesIsNotEnabled": "Pasting files is not enabled.",
    "FindOutMore": "Find out more",
    "NoDataBundles": "No Entity or Content for data bundles found. Select some Entity or Content on MetaData Export Decorator."
  },
  "LangMenu": {
    "Translate": "Translate",
    "TranslateAll": "Translate all",
    "AllFields": "all fields",
    "AutoTranslateAll": "Auto-Translate all",
    "AutoTranslate": "Auto-Translate",
    "NoTranslate": "Don't translate",
    "NoTranslateAll": "Don't translate any",
    "Link": "Link to other language",
    "UseDefault": "auto (default)",
    "InAllLanguages": "in all languages",
    "MissingDefaultLangValue": "please create value in the default language {{languages}} before translating",
    "In": "in {{languages}}",
    "From": "from {{languages}}",
    "NotTranslatable": "Not translatable",
    "SnackBar": {
      "Title": "⚠️ Warning: You are using a test account",
      "Body": "You are using a demo Google Account to translate. This can stop working at any time. <br>You <strong>must</strong> register your own API key. It's free for 500'000 words per month. See <a href='https://go.2sxc.org/translate-api-key' target='_blank'>docs</a>."
    },
    "NoTranslatableFieldsFound": "No fields found which can be translated/unlocked.",
    "NoAutoTranslatableFieldsFound": "No fields found which can be auto-translated.",
    "Dialog": {
      "Title": "Translate {{name}}",
      "Intro": "You can do many things when translating, like linking languages together.",
      "NoTranslate": {
        "Title": "Don't Translate",
        "Body": "Use value in primary language {{primary}}"
      },
      "FromPrimary": {
        "Title": "Translate from: {{primary}}",
        "Body": "Begin translation with with the value in the primary language"
      },
      "FromOther": {
        "Title": "Translate from: ...",
        "Body": "Begin translation with the value from another language",
        "Subtitle": "Language to translate from"
      },
      "LinkReadOnly": {
        "Title": "Inherit from other language (read-only)",
        "Body": "Inherit value from another language",
        "Subtitle": "Language to inherit from"
      },
      "LinkShared": {
        "Title": "Share with another language (read/write)",
        "Body": "Link languages together to use the same editable value",
        "Subtitle": "Language to share with"
      },
      "PickLanguageIntro": "Only languages with content can be selected.",
      "AutoTranslateAll": {
        "Title": "Auto-Translate all Fields using Google Translate",
        "Body": "Only fields that are not yet translated will be auto-translated. Note that not all fields will be translated - see <a href='https://go.2sxc.org/translate' target='_blank'>docs</a>.",
        "NoContent": "no content",
        "AllTranslatableFields": "all translatable fields",
        "Of": "of",
        "TranslatableFields": "translatable fields",
        "Empty": "empty"
      },
      "CantAutoTranslate": "Can't auto-translate",
      "NoAutoTranslatableFields": "There are no auto-translatable fields in this form",
      "ThisFieldIsNotAutoTranslatable": "This field is not auto-translatable"
    }
  },
  "Errors": {
    "UnsavedChanges": "You have unsaved changes.",
    "SaveErrors": "To save the form, please fix the following errors:",
    "FormulaConfiguration": "There is an error in form configuration. Please report this to admin",
    "FormulaCalculation": "There was an error in form calculations. Please report this to admin"
  },
  "General": {
    "Buttons": {
      "NotSave": "Discard changes",
      "Save": "Save",
      "Cancel": "Cancel"
    },
    "CopyHint": "This is a copy and will be saved as a new item",
    "ReadOnlyHint": {
      "Form": "Form is read only",
      "Language": "Language is read only"
    }
  },
  "Data": {
    "Delete.Question": "Delete '{{title}}' ({{id}})?"
  },
  "ItemCard": {
    "DefaultTitle": "Edit item",
    "SlotUsedTrue": "This item is open for editing. Click here to lock / remove it and revert to default.",
    "SlotUsedFalse": "This item is locked and will stay empty/default. The values are shown for your convenience. Click here to unlock if needed."
  },
  "ValidationMessage": {
    "NotValid": "Not valid",
    "Required": "This is required",
    "RequiredShort": "required",
    "Min": "This value should be {{Min}} or higher",
    "Max": "This value should be {{Max}} or lower",
    "MinNoItems": "Number of items should be {{AllowMultiMin}} or higher",
    "MaxNoItems": "Number of items should be {{AllowMultiMax}} or lower",
    "Pattern": "Please match the requested format",
    "Decimals": "This number can have up to {{Decimals}} decimal places",
    "JsonError": "JSON is not valid",
    "JsonWarning": "JSON is not valid"
  },
  "Fields": {
    "Picker": {
      "Choose": "Add existing item",
      "New": "Create new",
      "Empty": "Empty",
      "EmptySlot": "empty slot",
      "EntityNotFound": "(item not found)",
      "DragMove": "Drag to reorder the list",
      "Edit": "Edit this item",
      "Remove": "Remove from list",
      "Delete": "Delete",
      "Loading": "Loading...",
      "Search": "Search",
      "AddItemBelow": "Add item below",
      "AddExistingItem": "Add existing item",
      "OpenMultiselect": "Open multiselect",

      "FilterNoResults": "search for '{{search}}' returned no results",

      "QueryNoItems": "No items found",
      "QueryError": "Error: An error occurred while executing the query. {{error}} See the console for more information.",
      "QueryErrorNoData": "Error: An error occurred while executing the query. No data found. See the console for more information.",
      "QueryStreamNotFound": "Error: The query did not return a stream named",
      "QueryNotDefined": "Error: can't load data, query is not configured"
    },
    "Hyperlink": {
      "Default": {
        "Tooltip": "Drop files here to auto upload. For help see 2sxc.org/help?tag=adam. ADAM - sponsored with ♡ by 2sic.com",
        "Sponsor": "ADAM - sponsored with ♡ by 2sic.com",
        "Fullscreen": "Open in fullscreen",
        "AdamTip": "Quick upload using ADAM",
        "PageTip": "Pick a page",
        "MoreOptions": "More...",
        "MenuAdam": "Upload file with Adam",
        "MenuPage": "Page Picker",
        "MenuImage": "Image Manager",
        "MenuDocs": "File Manager"
      },
      "AdamFileManager": {
        "Name": "ADAM",
        "UploadLabel": "Upload to",
        "UploadTip": "Quick upload using ADAM",
        "UploadPasteLabel": "Paste Image",
        "UploadPasteFocusedLabel": "Press CTRL + V",
        "UploadPasteTip": "Click here and press CTRL + V to paste image from clipboard",
        "NewFolder": "New Folder",
        "NewFolderTip": "Create a new folder",
        "BackFolder": "Back",
        "BackFolderTip": "Return to previous folder",
        "Show": "Open in new tab",
        "ImageSettings": "Image settings",
        "ImageSettingsUnavailable": "Image settings not available. The file is not an image or it doesn't belong to this item",
        "ImageSettingsDisabled": "Image settings are disabled for this field",
        "Edit": "Rename",
        "RenameQuestion": "Rename file / folder to:",
        "Delete": "Delete",
        "DeleteQuestion": "Are you sure you want to delete this file?",
        "Hint": "drop or paste files",
        "HelpTooltip": "ADAM is the Automatic Digital Assets Manager - click to discover more",
        "SponsorLine": "is sponsored with ♡ by"
      },
      "PagePicker": {
        "Title": "Select a web page",
        "HiddenOrSystemPageWarning": "This page is hidden or a system page. Are you sure you want to link to it?"
      }
    },
    "DateTime": {
      "Open": "Open calendar",
      "OpenCalender": "Open calendar",
      "OpenTime": "Pick time"
    },
    "String": {
      "Dropdown": "Pick existing value",
      "Freetext": "Manual entry"
    },
    "TemplatePicker": {
      "NotSelected": "(no file selected)",
      "NewTemplate": "Create a new file"
    }
  },
  "ManageContentList": {
    "Title": "Manage item list",
    "Description": "You can manage the list header here (if it is defined).",
    "NoHeader": "(this list has no header)",
    "SortItems": "Sort the items by dragging as needed.",
    "SortLotsOfItems": "Sort the items by dragging and scrolling with mouse wheel as needed.",
    "ListHasBeenReordered": "⚠️ The list has been reordered. Because of this, certain buttons are disabled until you save your changes.",
    "ConfirmRemove": "This will remove this content-item from this list, but not delete it (so you can add it again later). \nSee 2sxc.org/help for more. \n\nOk to remove?"
  },
  "SharingOrInheriting": {
    "TitleAddFieldsWhichInherit": "Add Fields which Inherits Configuration",
    "TitleNone": "Share or inherit?",
    "TitleSharing": "Field Metadata Sharing",
    "TitleInheriting": "Field Inherits Metadata",
    "AddSharedField": "Add shared fields",
    "PickFieldsToInheritFrom": "Pick original fields which we'll inherit from"
  },
  "Extension.TinyMce": {
    "Link.AdamFile": "Link ADAM file (recommended)",
    "Link.AdamFile.Tooltip": "Link using ADAM - just drop files using the Automatic Digital Assets Manager",
    "Image.AdamImage": "Insert ADAM image (recommended)",
    "Image.AdamImage.Tooltip": "Image from ADAM - just drop files using the Automatic Digital Assets Manager",
    "Image.PasteImage": "Paste image",
    "Image.PasteImage.Tooltip": "Paste image from clipboard (Ctl+V)",
    "Image.PasteImage.Message": "Use Ctrl+V (Mac: Cmd+V) to paste an image from clipboard",
    "Link.DnnFile": "Link DNN file",
    "Link.DnnFile.Tooltip": "Link a DNN file (all files, slow)",
    "Image.DnnImage": "Insert DNN image",
    "Image.DnnImage.Tooltip": "Image from DNN file storage (all files, slow)",
    "Link.Page": "Link to another page",
    "Link.Page.Tooltip": "Link a page from the current site",
    "Link.Anchor.Tooltip": "Anchor to link to using .../page#anchorname",
    "SwitchMode.Tooltip": "Switch work mode",
    "SwitchMode.Pro": "Switch to advanced mode",
    "SwitchMode.Standard": "Switch to standard mode",
    "SwitchMode.Expand": "Switch to full screen",
    "H1": "H1",
    "H2": "H2",
    "H3": "H3",
    "H4": "H4",
    "H5": "H5",
    "H6": "H6",
    "Paragraph": "Paragraph",
    "ContentBlock.Add": "Add app or content block",
    "ContentDivision.Add": "Add content division",
    "RichImages.Ratio100.Label": "Width 100%",
    "RichImages.Ratio100.Tooltip": "Make width cover entire area of screen (desktop and mobile)",
    "RichImages.RatioXofY.Label": "Width {0}/{1}",
    "RichImages.RatioXofY.Tooltip": "Make width {0}/{1} of the desktop width; mobile will auto-adjust",
    "Splitter.0": "Gap with no height, just ensure the next content starts on a new line",
    "Splitter.s": "Gap with a small height",
    "Splitter.m": "Gap with a medium height",
    "Splitter.l": "Gap with a large height"
  },
  "Features": {
    "NotActivated": "The feature '{{name}}' is not activated - find out more",
    "NotActive": "Not active",
    "Active": "Active",
    "Status": "Status",
    "SecurityRating": "Security rating",
    "FeatureNotActivated": "This cool feature is not enabled",
    "ClickToFindOutMore": "Click to find out more...",
    "FindOutMore": "Find out more",
    "Tooltip": "Feature: {{name}} ({{nameId}})",
    "Close": "Close"
  },
  "License-EditUi-Warning-Label": "Preview",
  "License-EditUi-Warning-Tooltip": "You're using features which are not licensed.",
  "AppAdmin": {
    "ErrorTooManyAppSettings": "Found too many settings for {{ errComponent }}.",
    "ErrorTooManyAppResources": "Found too many resources for {{ errComponent }}.",
    "ErrorNoAppSettings": "Found no settings for {{ errComponent }}.",
    "ErrorNoAppResources": "Found no resources for {{ errComponent }}."
  },
  "LanguageSettings": {
    "Title": "Language Settings",
    "Intro": "By default, buttons and labels will use the current language. Here you can choose a different behavior.",
    "Subtitle": "Your Language Preferences",
    "UseCurrentLanguage": "Use current language (default)",
    "ButtonsAndControlsOptionLabel": "Buttons and Controls",
    "ButtonsAndControlsTooltip": "Preferred language for buttons and controls",
    "LabelsAndTextsOptionLabel": "Form Labels and Help Texts",
    "LabelsAndTextsTooltip": "Preferred language for texts and labels in the form",
    "TranslatePrimaryOptionLabel": "Show Language Menus in Primary Language",
    "TranslatePrimaryTooltip": "Shows the Language Menus also in the Primary Language",
    "ActivatedOption": "Activated",
    "DeactivatedOption": "Deactivated"
  }
}