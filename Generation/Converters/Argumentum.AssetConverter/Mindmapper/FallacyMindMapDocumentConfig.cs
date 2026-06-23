using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Xsl;
using ImageMagick;
using Spectre.Console;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Color = System.Drawing.Color;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Mindmapper
{
    public class FallacyMindMapDocumentConfig : FallacyDocumentConfigBase
	{
		public MindMapFormat Format { get; set; } = MindMapFormat.Freemind;
		// Trying to making sure the assemblies get published
		private static readonly System.Diagnostics.StackTrace temp1 = new();
		private static readonly System.Drawing.Color temp2 = Color.AliceBlue;

		//public string DocumentName { get; set; } = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm";



		const string DefaultTitleExpression = @"{item.TextFr}";

		public string TitleExpression { get; set; } = DefaultTitleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> TitleFunc
		{
			get
			{
				return item =>
				{

					var expression = TitleExpression;
					var title = expression.Interpolate(new Dictionary<string, object>() { { "item", item } });
					if (AddNodePath)
					{
						title = $"{item.Path} - {title}";
					}
					return title;
				};
			}
		}

		public bool AddNodePath { get; set; } = false;


		const string DefaultFamilleExpression = @"{item.Famille}";
		public string FamilleExpression { get; set; } = DefaultFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> FamilleFunc
		{
			get
			{
				return item => FamilleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.TextFr}";
			}
		}


		const string DefaultSousFamilleExpression = @"{item.SousFamille}";
		public string SousFamilleExpression { get; set; } = DefaultSousFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> SousFamilleFunc
		{
			get
			{
				return item => SousFamilleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.TextFr}";
			}
		}


		const string DefaultSoussousFamilleExpression = @"{item.Soussousfamille}";
		public string SoussousFamilleExpression { get; set; } = DefaultSoussousFamilleExpression;

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> SoussousFamilleFunc
		{
			get
			{
				return item => SoussousFamilleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.TextFr}";
			}
		}

		public string DescriptionExpression { get; set; } =
@"
<p>
    {HttpUtility.HtmlEncode(item.DescFr)}
</p>
";

		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> DescFunc
		{
			get
			{
				return item => DescriptionExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"<font size='4'>{HttpUtility.HtmlEncode(item.Description)}</font>";
			}
		}


		public string CardExpression { get; set; } =
			@"
<p>
    <img src=""{mindMap.GetThumbnailsPath(item)}"" width=""60"" height=""60""/>" + DefaultTitleExpression + @"
</p>
";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> CardFunc
		{
			get
			{
				return item => CardExpression.Replace("fallacy.", "item.").Replace("fallacy", "item").Interpolate(new Dictionary<string, object>() { { "mindMap", this }, { "item", item } }); // $"<font size='4'>{HttpUtility.HtmlEncode(item.Description)}</font>";
			}
		}



		public string ExampleExpression { get; set; } =
@"
<p>
    <i>{HttpUtility.HtmlEncode(item.ExampleFr)}</i>
</p>
";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> ExampleFunc
		{
			get
			{
				return item => ExampleExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"<i>{HttpUtility.HtmlEncode(item.Example)}</i>";
			}
		}



		public string LinkExpression { get; set; } = @"{item.LinkFrFallback}";


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> LinkFunc
		{
			get
			{
				return item => LinkExpression.Replace("fallacy.", "item.").Replace("fallacy", "item").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.Link}";
			}
		}






		public string ThumbnailsPathExpression { get; set; } = @"Target/Images/density-0/Fallacies-Web-Thumbnails/argumentum_{item.Path}_{item.Text.ToLower().Replace("" "",""_"")}.png";


		public string GetThumbnailsPath(IMindMapItem item)
		{
			return ThumbnailsPathFunc(item);
		}


		private Func<IMindMapItem, string> _Thumbnails;
		


		[IgnoreDataMember]
		[JsonIgnore]
		public Func<IMindMapItem, string> ThumbnailsPathFunc
		{
			get
			{
				return _Thumbnails ??= item =>
					ThumbnailsPathExpression.Replace("fallacy.", "item.").Interpolate(new Dictionary<string, object>() { { "item", item } }); // $"{item.Text}";
			}
			set
			{
				_Thumbnails = value;
			}
		}


		public int NbBranchesRight { get; set; } = 2;

		public Dictionary<int, string> Colors { get; set; } = new Dictionary<int, string>()
		{
			{1, "#8605ab"},
			{2, "#ff66eb"},
			{3, "#08af93"},
			{4, "#8dc801"},
			{5, "#0054a4"},
			{6, "#ffc307"},
			{7, "#dc0f0a"},

		};


		public List<int> FontSizes { get; set; } = new List<int>(new[] { 60, 60, 50, 40, 30, 30, 25, 23, 23, 23, 23 });


		public List<int> EdgeSizes { get; set; } = new List<int>(new[] { 20, 10, 5, 1});


		public bool InsertCardsThumbnails { get; set; }


		public string ThumbnailsCardSetName { get; set; }

		public string ThumbnailsFileNamePattern { get; set; } = "_{item.Path}..";


		public List<SVGFreemindMap> SVGMaps { get; set; } = new List<SVGFreemindMap>();

		public bool KeepOriginalSVG { get; set; } = true;

		public CrossLink CrossLinks { get; set; } = CrossLink.None;

		public string MatchThumbnailsName(string targetDirectory, IMindMapItem item)
		{
			var fileNames = Directory.GetFiles(targetDirectory);
			var thumbnailsFallacyPattern = ThumbnailsFileNamePattern.Replace("fallacy.", "item.").Interpolate(
				new Dictionary<string, object>() { { "item", item } });
			return fileNames.FirstOrDefault(fileName => fileName.Contains(thumbnailsFallacyPattern));
		}


		public override async Task GenerateMindMapFile(IList objects, AssetConverterConfig config, string targetDirectory, string language)
		{
			var mindMapItems = objects.Cast<IMindMapItem>().ToList();
			if (string.IsNullOrEmpty(language))
				language = config.LocalizationConfig.DefaultLanguage;


			var fileName = DocumentName;
			if (!string.IsNullOrEmpty(targetDirectory))
			{
				fileName = Path.Combine(targetDirectory, fileName);

			}
			var documentPath = Path.Combine(targetDirectory, DocumentName);

			CreateFreemindmap(mindMapItems, config, language, documentPath, fileName);

			//Task.Run(async () => await ProcessSVGFiles(fallacies, fileName, webBasedGeneratorConfig, webBasedGeneratorConfig.EnableSVGPrompt)).GetAwaiter().GetResult() ;
			await ProcessSvgFilesAsync(mindMapItems, fileName, config, config.EnableSVGPrompt, language);
		}

		private void CreateFreemindmap(IList<IMindMapItem> mindMapItems, AssetConverterConfig config, string language, string documentPath, string fileName)
		{
			if (File.Exists(documentPath) && !config.OverwriteExistingDocs)
			{
				Logger.Log($"Skip existing Mindmap: {documentPath}");
			}
			else
			{
				Logger.Log($"Creating Freemind mind map {DocumentName}");
				FreemindMap freemindMap;
				if (Format == MindMapFormat.Freeplane)
				{
					freemindMap = new FreeplaneMap();
				}
				else
				{
					freemindMap = new FreemindMap();
				}
				
				var nodesByPath = new Dictionary<string, Node>(mindMapItems.Count);
				CreateMindMapNodes(freemindMap, mindMapItems, nodesByPath, config, language);


				SerializeMindMapAsync(freemindMap, fileName);

				var svgPath = Path.ChangeExtension(fileName, "svg");
				// Run on thread pool to isolate WinForms SyncContext installed by SendKeys
				Task.Run(() => TryAutomateSvgConversion(fileName, svgPath, config, config.EnableSVGPrompt)).GetAwaiter().GetResult();
				// Clean up any WinForms SyncContext that may have been installed
				System.Threading.SynchronizationContext.SetSynchronizationContext(null);
			}
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool BringWindowToTop(IntPtr hWnd);
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern uint GetCurrentThreadId();
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);
		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr OpenInputDesktop(uint dwFlags, bool fInherit, uint dwDesiredAccess);
		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		private static extern bool SetThreadDesktop(IntPtr hDesktop);
		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		private static extern bool CloseDesktop(IntPtr hDesktop);
		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr GetThreadDesktop(uint dwThreadId);
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private static extern bool GetUserObjectInformation(IntPtr hObj, int nIndex, System.Text.StringBuilder pvInfo, uint nLength, out uint lpnLengthNeeded);

		private static string GetDesktopName(IntPtr hDesktop)
		{
			if (hDesktop == IntPtr.Zero) return "<null>";
			var sb = new System.Text.StringBuilder(256);
			return GetUserObjectInformation(hDesktop, 2 /* UOI_NAME */, sb, (uint)sb.Capacity, out _) ? sb.ToString() : "<unknown>";
		}

		/// <summary>
		/// Switches the calling thread to the interactive input desktop (WinSta0\Default).
		/// Returns the new desktop handle (to CloseDesktop later) or IntPtr.Zero if the thread
		/// was already on the input desktop or attaching failed. Required when our process runs
		/// on a non-interactive desktop (e.g. Service-0x0-3e7$\Default): without this switch,
		/// GetForegroundWindow is NULL on our desktop, SetForegroundWindow is a no-op, and
		/// SendKeys.SendWait routes keystrokes to a desktop with no visible window.
		/// </summary>
		// Tracks whether SetThreadDesktop has already been called on the pipeline's main thread.
		// SetThreadDesktop refuses (Win32 ERROR_BUSY = 170) on subsequent calls because the thread
		// retains hooks/windows from the previous SendKeys run — but that's fine: we're already on
		// the right desktop, so we can no-op silently.
		private static bool _threadAttachedToInputDesktop = false;

		private static IntPtr TryAttachToInteractiveDesktop()
		{
			if (_threadAttachedToInputDesktop)
				return IntPtr.Zero; // already on input desktop from a previous call

			var hCurrent = GetThreadDesktop(GetCurrentThreadId());
			var hInput = OpenInputDesktop(0, false, 0x10000000 /* GENERIC_ALL */);
			if (hInput == IntPtr.Zero)
			{
				Logger.LogWarning($"OpenInputDesktop failed (Win32 err {System.Runtime.InteropServices.Marshal.GetLastWin32Error()}). Current desktop: {GetDesktopName(hCurrent)}");
				return IntPtr.Zero;
			}
			if (!SetThreadDesktop(hInput))
			{
				int err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				// 170 = ERROR_BUSY: thread already has windows on its current desktop. If the names match,
				// we're already on the input desktop and can proceed silently.
				if (err == 170 && GetDesktopName(hCurrent) == GetDesktopName(hInput))
				{
					_threadAttachedToInputDesktop = true;
					CloseDesktop(hInput);
					return IntPtr.Zero;
				}
				Logger.LogWarning($"SetThreadDesktop failed (Win32 err {err}). From {GetDesktopName(hCurrent)} to {GetDesktopName(hInput)}");
				CloseDesktop(hInput);
				return IntPtr.Zero;
			}
			Logger.Log($"Attached thread to interactive desktop: {GetDesktopName(hCurrent)} → {GetDesktopName(hInput)}");
			_threadAttachedToInputDesktop = true;
			return hInput;
		}

		private static string DescribeWindow(IntPtr hWnd)
		{
			if (hWnd == IntPtr.Zero) return "<null>";
			var sb = new System.Text.StringBuilder(256);
			GetWindowText(hWnd, sb, sb.Capacity);
			return $"hWnd=0x{hWnd.ToInt64():X} title='{sb}'";
		}

		/// <summary>
		/// Forces a window to the foreground reliably. Plain SetForegroundWindow is silently
		/// refused by Windows when the calling process isn't already foreground, so we attach
		/// our input queue to the current foreground thread (AttachThreadInput) for the call,
		/// then verify GetForegroundWindow actually points at the target before returning.
		/// </summary>
		private static bool ForceForeground(IntPtr hWnd)
		{
			ShowWindow(hWnd, 9); // SW_RESTORE
			for (int attempt = 0; attempt < 5; attempt++)
			{
				uint fgThread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
				uint thisThread = GetCurrentThreadId();
				bool attached = false;
				try
				{
					if (fgThread != 0 && fgThread != thisThread)
						attached = AttachThreadInput(thisThread, fgThread, true);
					BringWindowToTop(hWnd);
					SetForegroundWindow(hWnd);
					ShowWindow(hWnd, 9);
				}
				finally
				{
					if (attached)
						AttachThreadInput(thisThread, fgThread, false);
				}
				Thread.Sleep(400);
				if (GetForegroundWindow() == hWnd)
					return true;
			}
			var fg = GetForegroundWindow();
			Logger.LogWarning($"ForceForeground failed. Target {DescribeWindow(hWnd)} | Actual foreground {DescribeWindow(fg)}");
			return fg == hWnd;
		}

		// Mutex: only one FreeMind GUI automation at a time (shared across Fallacy + Virtue)
		private static readonly object FreeMindLock = new object();

		private bool TryAutomateSvgConversion(string sourceMmPath, string destinationSvgPath, AssetConverterConfig config, bool isInteractive = true)
		{
			return TryFreeMindSvgExport(sourceMmPath, destinationSvgPath, config);
		}

		/// <summary>
		/// Exports a .mm file to SVG by launching FreeMind GUI and automating menu navigation.
		/// FreeMind 1.0.1 File menu (FR): Nouveau, Ouvrir, Fermer, Enregistrer, Enregistrer sous,
		/// ---separator---, Exporter▸, Importer▸, ---separator---, Mise en page, Imprimer, Aperçu,
		/// ---separator---, Quitter.
		/// Export submenu: Branche..., Using XSLT..., As HTML..., As XHTML..., As HTML template...,
		///                 ---separator---, As PDF..., Branch As PDF..., As SVG...
		/// </summary>
		internal static bool TryFreeMindSvgExport(string sourceMmPath, string destinationSvgPath, AssetConverterConfig config)
		{
			lock (FreeMindLock)
			{
				return TryFreeMindSvgExportCore(sourceMmPath, destinationSvgPath, config);
			}
		}

		/// <summary>
		/// Sends keystrokes to the foreground window, swallowing the spurious
		/// Win32Exception "L'opération a réussi" (NativeErrorCode 0) that SendKeys.SendWait
		/// raises as a false negative — see Mindmapper/xslt/Export-FreeMindSvg.ps1.
		/// Without this guard the very first keystroke aborts the whole export.
		/// </summary>
		private static void SendKeysSafe(string keys)
		{
			try
			{
				System.Windows.Forms.SendKeys.SendWait(keys);
			}
			catch (System.ComponentModel.Win32Exception ex)
			{
				// SendKeys.SendWait raises a spurious Win32Exception "L'opération a réussi."
				// (NativeErrorCode 0) as a false negative — the keystroke is actually delivered.
				// The sibling PS1 script (Mindmapper/xslt/Export-FreeMindSvg.ps1) swallows it the same way.
				Logger.Log($"SendKeys spurious Win32Exception ignored ({ex.NativeErrorCode}): {ex.Message}");
			}
		}

		private static void CleanFreeMindRecoveryFiles()
		{
			var freemindUserDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".freemind");
			if (!Directory.Exists(freemindUserDir)) return;
			foreach (var f in Directory.GetFiles(freemindUserDir, "FM_*.mm"))
			{
				try { File.Delete(f); Logger.Log($"Deleted FreeMind recovery file: {Path.GetFileName(f)}"); } catch { }
			}
			foreach (var f in Directory.GetFiles(freemindUserDir, "*.lck"))
			{
				try { File.Delete(f); } catch { }
			}
			ClearFreeMindAutoOpenedTabs(freemindUserDir);
		}

		/// <summary>
		/// Clears the lastOpened / mindmap_last_state_map_storage keys in auto.properties so that
		/// FreeMind opens ONLY the .mm passed on the CLI, not 5 stale tabs from previous runs.
		/// Without this, the keystrokes target whichever tab FreeMind happens to focus on first,
		/// and the SVG export silently writes to the wrong file (or nothing at all).
		/// </summary>
		private static void ClearFreeMindAutoOpenedTabs(string freemindUserDir)
		{
			var propsPath = Path.Combine(freemindUserDir, "auto.properties");
			if (!File.Exists(propsPath)) return;
			try
			{
				var lines = File.ReadAllLines(propsPath);
				bool changed = false;
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].StartsWith("lastOpened=") && lines[i] != "lastOpened=")
					{
						lines[i] = "lastOpened=";
						changed = true;
					}
					else if (lines[i].StartsWith("mindmap_last_state_map_storage="))
					{
						lines[i] = "mindmap_last_state_map_storage=";
						changed = true;
					}
				}
				if (changed)
				{
					File.WriteAllLines(propsPath, lines);
					Logger.Log("Cleared FreeMind auto-restored tabs from auto.properties");
				}
			}
			catch (Exception ex)
			{
				Logger.LogWarning($"Could not clear FreeMind auto.properties: {ex.Message}");
			}
		}

		private static void KillAllFreeMind()
		{
			// Kill ALL javaw processes — we deliberately don't filter on MainWindowTitle.Contains("FreeMind")
			// because a FreeMind that's still loading a large .mm has an empty/unstable title, and
			// would otherwise survive between iterations and steal focus from the next run.
			foreach (var jp in Process.GetProcessesByName("javaw"))
			{
				try
				{
					Logger.Log($"Killing javaw pid={jp.Id} title='{jp.MainWindowTitle}'");
					jp.Kill();
					jp.WaitForExit(5000);
				}
				catch { }
			}
			CleanFreeMindRecoveryFiles();
		}

		private static bool TryFreeMindSvgExportCore(string sourceMmPath, string destinationSvgPath, AssetConverterConfig config)
		{
			// FreeMindPath is machine-specific (absolute Windows path), so the C# default stays "".
			// It resolves at runtime from (1) config.FreeMindPath, then (2) the ARGUMENTUM_FREEMIND_PATH
			// env var. This avoids hardcoding a path that would break on machines where FreeMind isn't
			// installed at that exact location, while still letting an attended run find it.
			var freemindPath = config.FreeMindPath;
			if (string.IsNullOrEmpty(freemindPath))
			{
				freemindPath = Environment.GetEnvironmentVariable("ARGUMENTUM_FREEMIND_PATH");
			}
			if (string.IsNullOrEmpty(freemindPath) || !File.Exists(freemindPath))
			{
				Logger.LogWarning($"FreeMind not found (config.FreeMindPath='{config.FreeMindPath}', env ARGUMENTUM_FREEMIND_PATH unset or invalid). Skipping GUI export.");
				return false;
			}

			// SVG will be saved next to .mm with same name (FreeMind default behavior)
			var generatedSvgPath = System.IO.Path.ChangeExtension(sourceMmPath, ".svg");
			var svgTimeBefore = File.Exists(generatedSvgPath) ? File.GetLastWriteTimeUtc(generatedSvgPath) : DateTime.MinValue;

			// Switch our thread to the interactive input desktop (WinSta0\Default) so that
			// GetForegroundWindow / SetForegroundWindow / SendKeys actually reach the FreeMind
			// window. Without this, a process spawned from a non-interactive context sees a
			// NULL foreground and keystrokes are dropped on its phantom desktop.
			IntPtr hInputDesktop = TryAttachToInteractiveDesktop();
			try
			{
				// 1. Clean slate
				KillAllFreeMind();
				Thread.Sleep(2000);

				// 2. Launch FreeMind
				Logger.Log($"Launching FreeMind: {System.IO.Path.GetFileName(sourceMmPath)}");
				var process = Process.Start(new ProcessStartInfo
				{
					FileName = freemindPath,
					Arguments = $"\"{sourceMmPath}\"",
					UseShellExecute = true
				});
				if (process == null) { Logger.LogWarning("Failed to start FreeMind."); return false; }

				// 3. Poll for window (up to 90s) — wait for title to contain the .mm filename.
				// Large mindmaps (cards-per-fallacy variants ~1MB) take 30-60s to load on FreeMind 1.0.1.
				Process freemindProcess = null;
				var mmFileName = System.IO.Path.GetFileName(sourceMmPath);
				Logger.Log("Waiting for FreeMind window...");
				for (int i = 0; i < 90 && freemindProcess == null; i++)
				{
					Thread.Sleep(1000);
					foreach (var jp in Process.GetProcessesByName("javaw"))
					{
						try
						{
							jp.Refresh();
							if (jp.MainWindowTitle.Contains(mmFileName))
							{
								freemindProcess = jp;
								break;
							}
						}
						catch { }
					}
				}
				if (freemindProcess == null)
				{
					Logger.LogWarning("FreeMind window not found after 90s.");
					try { process.Kill(); } catch { }
					KillAllFreeMind(); // make sure no stray javaw survives to pollute the next iteration
					return false;
				}
				Logger.Log($"FreeMind ready: '{freemindProcess.MainWindowTitle}'");
				Thread.Sleep(5000); // let rendering finish

				// 4. Focus window — robust foreground via AttachThreadInput, verified
				if (ForceForeground(freemindProcess.MainWindowHandle))
					Logger.Log("FreeMind window focused.");
				else
					Logger.LogWarning("Could not confirm FreeMind window focus — keystrokes may misfire.");
				Thread.Sleep(2000);

				// 5. Menu navigation: Alt+F → 8×DOWN → RIGHT → 12×DOWN → ENTER → ENTER → ENTER
				Logger.Log("Sending keystrokes: Alt+F, 8×DOWN, RIGHT, 12×DOWN, ENTER, ENTER, ENTER");

				// Re-assert focus right before typing, in case it drifted during the sleep
				ForceForeground(freemindProcess.MainWindowHandle);
				SendKeysSafe("{ESC}");
				Thread.Sleep(500);

				SendKeysSafe("%f");
				Thread.Sleep(2000);

				for (int i = 0; i < 8; i++)
				{
					SendKeysSafe("{DOWN}");
					Thread.Sleep(300);
				}

				SendKeysSafe("{RIGHT}");
				Thread.Sleep(1500);

				for (int i = 0; i < 12; i++)
				{
					SendKeysSafe("{DOWN}");
					Thread.Sleep(300);
				}

				// ENTER = select "En SVG..."
				SendKeysSafe("{ENTER}");
				Thread.Sleep(3000);

				// ENTER = confirm save dialog (default path = same dir as .mm)
				SendKeysSafe("{ENTER}");
				Thread.Sleep(3000);

				// ENTER = overwrite confirmation (if file exists)
				SendKeysSafe("{ENTER}");
				Thread.Sleep(5000);

				// 6. Check result: file must exist with a newer timestamp
				bool svgGenerated = File.Exists(generatedSvgPath)
					&& File.GetLastWriteTimeUtc(generatedSvgPath) > svgTimeBefore;

				if (svgGenerated)
					Logger.LogSuccess($"FreeMind SVG exported: {generatedSvgPath} ({new FileInfo(generatedSvgPath).Length / 1024} KB)");
				else
					Logger.LogWarning($"FreeMind SVG not detected at '{generatedSvgPath}'");

				// 7. Kill FreeMind + cleanup
				KillAllFreeMind();
				Thread.Sleep(3000);

				// 8. Move to destination if different from generated path
				if (svgGenerated && generatedSvgPath != destinationSvgPath)
				{
					var destDir = System.IO.Path.GetDirectoryName(destinationSvgPath);
					if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
						Directory.CreateDirectory(destDir);
					File.Move(generatedSvgPath, destinationSvgPath, true);
				}

				return svgGenerated;
			}
			catch (Exception ex)
			{
				Logger.LogWarning($"FreeMind SVG export error: {ex.Message}");
				KillAllFreeMind();
				return false;
			}
			finally
			{
				if (hInputDesktop != IntPtr.Zero)
					CloseDesktop(hInputDesktop);
			}
		}

		/// <summary>
		/// Fallback SVG conversion using XSLT stylesheets (mm2svg.xslt from tstephen/mindmap).
		/// Lower fidelity than FreeMind/Freeplane native rendering but works without GUI.
		/// </summary>
		internal static bool TryXsltSvgConversion(string sourceMmPath, string destinationSvgPath)
		{
			try
			{
				var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
				string xsltDir = null;

				var candidates = new[]
				{
					Path.Combine(assemblyDir, "Mindmapper", "xslt"),
					Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "Mindmapper", "xslt")),
					Path.GetFullPath(Path.Combine(assemblyDir, "..", "..", "..", "..", "..", "Mindmapper", "xslt")),
				};

				foreach (var candidate in candidates)
				{
					if (Directory.Exists(candidate) && File.Exists(Path.Combine(candidate, "mm2svg.xslt")))
					{
						xsltDir = candidate;
						break;
					}
				}

				if (xsltDir == null)
				{
					Logger.LogWarning("XSLT stylesheet mm2svg.xslt not found.");
					return false;
				}

				var xsltPath = Path.Combine(xsltDir, "mm2svg.xslt");
				Logger.Log($"Using XSLT fallback for SVG conversion: {xsltPath}");

				var xslt = new XslCompiledTransform();
				var xsltSettings = new XsltSettings(enableDocumentFunction: true, enableScript: false);
				xslt.Load(xsltPath, xsltSettings, new XmlUrlResolver());

				var destDir = Path.GetDirectoryName(destinationSvgPath);
				if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
					Directory.CreateDirectory(destDir);

				using (var writer = XmlWriter.Create(destinationSvgPath, xslt.OutputSettings))
				{
					xslt.Transform(sourceMmPath, writer);
				}

				if (File.Exists(destinationSvgPath) && new FileInfo(destinationSvgPath).Length > 0)
				{
					Logger.LogSuccess($"SVG via XSLT: {destinationSvgPath} ({new FileInfo(destinationSvgPath).Length / 1024} KB)");
					return true;
				}

				Logger.LogWarning("XSLT transformation produced empty output.");
				return false;
			}
			catch (Exception ex)
			{
				Logger.LogProblem($"XSLT SVG conversion failed: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Ensures the Groovy export script exists in Freeplane's user scripts directory.
		/// The script uses Freeplane's c.export() API for SVG export.
		/// Freeplane 1.13 uses %APPDATA%\Freeplane\1.13.x\scripts\ for user scripts.
		/// </summary>
		internal static void EnsureGroovyExportScript(AssetConverterConfig config)
		{
			// Freeplane user scripts go in %APPDATA%\Freeplane\{version}\scripts\
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var freeplaneUserDirs = new[] { "1.13.x", "1.12.x" };

			foreach (var versionDir in freeplaneUserDirs)
			{
				var scriptsDir = System.IO.Path.Combine(appData, "Freeplane", versionDir, "scripts");
				if (!Directory.Exists(scriptsDir))
					Directory.CreateDirectory(scriptsDir);

				var scriptPath = System.IO.Path.Combine(scriptsDir, "export_to_svg.groovy");
				var groovyScript = @"// Auto-generated by Argumentum pipeline for SVG export
// Usage: freeplane -Xexport_to_svg input.mm
def mapFile = node.map.file
if (mapFile != null) {
    def svgFile = new File(mapFile.path.replaceFirst('\\.mm$', '.svg'))
    c.export(node.map, svgFile, 'Scalable Vector Graphic (SVG) (.svg)', true)
}
";
				File.WriteAllText(scriptPath, groovyScript);
				Logger.Log($"Groovy export script ensured at: {scriptPath}");
			}
		}


		/* Removed duplicate TryXsltSvgConversion — the file-based version at line ~525 is used.
		/// <summary>
		/// XSLT-based mm→SVG conversion (DUPLICATE - DISABLED).
		/// Used as fallback when Freeplane GUI export is not available.
		/// Step 1: polyfill.xslt adds layout bounds to .mm nodes
		/// Step 2: mm2svg.xslt converts the polyfilled .mm to SVG
		/// </summary>
		internal static bool TryXsltSvgConversion(string sourceMmPath, string destinationSvgPath)
		{
			try
			{
				var assembly = Assembly.GetExecutingAssembly();
				var resourcePrefix = "Argumentum.AssetConverter.Mindmapper.xslt.";

				using var polyfillStream = assembly.GetManifestResourceStream(resourcePrefix + "polyfill.xslt");
				using var mm2svgStream = assembly.GetManifestResourceStream(resourcePrefix + "mm2svg.xslt");

				if (polyfillStream == null || mm2svgStream == null)
				{
					Logger.LogWarning("XSLT resources not found in assembly. Skipping XSLT SVG conversion.");
					return false;
				}

				// Step 1: Apply polyfill.xslt to add layout bounds
				var polyfillTransform = new XslCompiledTransform();
				using (var reader = XmlReader.Create(polyfillStream))
				{
					polyfillTransform.Load(reader);
				}

				var polyfillResult = new MemoryStream();
				using (var inputReader = XmlReader.Create(sourceMmPath))
				{
					using var writer = XmlWriter.Create(polyfillResult, polyfillTransform.OutputSettings);
					polyfillTransform.Transform(inputReader, writer);
				}
				polyfillResult.Position = 0;

				// Step 2: Apply mm2svg.xslt to convert to SVG
				var mm2svgTransform = new XslCompiledTransform();
				using (var reader = XmlReader.Create(mm2svgStream))
				{
					mm2svgTransform.Load(reader);
				}

				var destDir = Path.GetDirectoryName(destinationSvgPath);
				if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
					Directory.CreateDirectory(destDir);

				using (var inputReader = XmlReader.Create(polyfillResult))
				{
					using var outputStream = File.Create(destinationSvgPath);
					using var writer = XmlWriter.Create(outputStream, mm2svgTransform.OutputSettings);
					mm2svgTransform.Transform(inputReader, writer);
				}

				if (File.Exists(destinationSvgPath) && new FileInfo(destinationSvgPath).Length > 0)
				{
					Logger.LogSuccess($"XSLT SVG conversion successful: {destinationSvgPath}");
					return true;
				}

				Logger.LogWarning($"XSLT SVG conversion produced empty file: {destinationSvgPath}");
				return false;
			}
			catch (Exception ex)
			{
				Logger.LogProblem($"XSLT SVG conversion failed: {ex.Message}");
				return false;
			}
		}
		*/


		private void CreateMindMapNodes(FreemindMap freemindMap, IList<IMindMapItem> mindMapItems, Dictionary<string, Node> nodesByPath, AssetConverterConfig config, string language)
		{
			var linkedItems = new HashSet<IMindMapItem>();

			foreach (var item in mindMapItems)
			{
				linkedItems.Add(item);
				if (string.IsNullOrEmpty(item.PK)) continue;

				var localPath = item.Path;

				List<(CrossLink crossLinkType, List<IMindMapItem> targets)> crossLinks = new();

				if (this.CrossLinks.HasFlag(CrossLink.Identity))
				{
					var identityItems = mindMapItems.Where(f => f.Text == item.Text && !linkedItems.Contains(f)).ToList();
					crossLinks.Add((CrossLink.Identity, identityItems));
				}

				var itemNode = CreateNode(item, config, language, crossLinks.ToArray());
				nodesByPath[localPath] = itemNode;

				var lastDotIndex = localPath.LastIndexOf('.');
				int familyNb;
				if (lastDotIndex > -1)
				{
					familyNb = int.Parse(item.Path[0].ToString(), CultureInfo.InvariantCulture);
					var parentPath = localPath[..lastDotIndex];
					var parentNode = nodesByPath[parentPath];
					parentNode.Nodes.Add(itemNode);
				}
				else
				{
					familyNb = int.Parse(localPath);
					AddNodeToFreemindMap(freemindMap, itemNode, familyNb);
				}

				SetNodeStyle(itemNode, item, familyNb);
			}
		}

		

		private Node CreateNode(IMindMapItem item, AssetConverterConfig config, string language, params (CrossLink crossLinkType, List<IMindMapItem> targets)[] crossLinks)
		{
			var itemNode = new Node { TEXT = TitleFunc(item) };
			itemNode.ID = item.Id;
			var link = LinkFunc(item);
			if (!string.IsNullOrEmpty(link))
			{
				itemNode.LINK = link;
			}

			var descRichContent = CreateRichContent(item);
			itemNode.Richcontents.Add(descRichContent);

			if (item.Carte.HasValue)
			{
				AddCardIcon(item, itemNode, config, language);
			}

			foreach (var crossLink in crossLinks)
			{
				foreach (var target in crossLink.targets)
				{
					var crossLinkNode = new Arrowlink();
					crossLinkNode.StartArrow = "Default";
					crossLinkNode.EndArrow = "Default";
					crossLinkNode.StartInclination = "892;0;";
					crossLinkNode.EndInclination = "892;0;";
					crossLinkNode.Destination = target.Id;

					switch (crossLink.crossLinkType)
					{
						case CrossLink.Identity:
							crossLinkNode.Color = "#dbffd6 ";
							break;
						case CrossLink.AppealTo:
							crossLinkNode.Color = "#ccffff";
							break;
						case CrossLink.Opposite:
							crossLinkNode.Color = "#ffcfcc";
							break;
						default:
							throw new ArgumentOutOfRangeException($"cross link type {crossLink.crossLinkType} unsupported");
					}
					itemNode.Arrowlinks.Add(crossLinkNode);

				}
			}


			return itemNode;
		}

		private Richcontent CreateRichContent(IMindMapItem item)
		{
			var descDoc = new XmlDocument();
			descDoc.LoadXml($"{DescFunc(item)}");

			var descRichContent = new Richcontent { TYPE = "NOTE" };
			descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

			descDoc.LoadXml($"{ExampleFunc(item)}");
			descRichContent.Html.Body.Elements.Add(descDoc.DocumentElement);

			return descRichContent;
		}

		private void AddNodeToFreemindMap(FreemindMap freemindMap, Node fallacyNode, int familyNb)
		{
			if (familyNb == 0)
			{
				fallacyNode.ID = "ID_706669011";
				freemindMap.Node = fallacyNode;
			}
			else
			{
				fallacyNode.POSITION = familyNb > NbBranchesRight && familyNb <= 6 ? "left" : "right";
				freemindMap.Node.Nodes.Add(fallacyNode);
			}
		}

		private void SetNodeStyle(Node node, IMindMapItem item, int familyNb)
		{
			if (item.Depth < FontSizes.Count)
			{
				node.Font = new Font() { Size = FontSizes[item.Depth].ToString() };
			}

			if (item.Depth < EdgeSizes.Count)
			{



				if (familyNb > 0)
				{
					node.Edge = new Edge() { WIDTH = EdgeSizes[item.Depth - 1].ToString(CultureInfo.InvariantCulture) };
					node.Edge.COLOR = Colors[familyNb];
					node.BACKGROUND_COLOR = HLSColor.GetLighterColor(Colors[familyNb]);
				}

				node.STYLE = "bubble";
			}
			else
			{
				node.STYLE = "fork";
				if (item.Depth == EdgeSizes.Count)
				{
					node.Edge = new Edge() { WIDTH = EdgeSizes[item.Depth - 1].ToString(CultureInfo.InvariantCulture) };
				}
			}

			if (item.Depth <= EdgeSizes.Count)
			{
				node.Font.BOLD = "true";
			}

			if (item.Depth >= EdgeSizes.Count)
			{
				node.COLOR = HLSColor.GetDarkerColor(Colors[familyNb]);
			}
		}

		private void AddCardIcon(IMindMapItem item, Node node, AssetConverterConfig assetConverterConfig, string language)
		{
			node.Icons.Add(new Icon() { BUILTIN = $"full-{item.Carte}" });

			if (InsertCardsThumbnails )
			{
				var cardSetConfig = assetConverterConfig.WebBasedGeneratorConfig.CardSets.FirstOrDefault(c => c.Name == this.ThumbnailsCardSetName, null);
				if (cardSetConfig != null)
				{
					this.ThumbnailsPathFunc = objItem =>
					{
						var cardSetDirectory = ImageHelper.GetImageFolder(assetConverterConfig, this, language, ThumbnailsCardSetName);
						var imageFileName = MatchThumbnailsName(cardSetDirectory, item);
						if (string.IsNullOrEmpty(imageFileName))
						{
							Logger.LogProblem($"No thumbnail for item {TitleFunc(item)} in directory {cardSetDirectory}");
						}
						else
						{
							var targetDirectory = assetConverterConfig.GetDocumentDirectory(language);
							imageFileName = imageFileName.GetRelativePathFrom(targetDirectory);
						}
						return imageFileName;
					};
				}

				var cardDoc = new XmlDocument();
				cardDoc.LoadXml($"{CardFunc(item)}");
				var cardRichContent = new Richcontent();
				node.Richcontents.Add(cardRichContent);
				cardRichContent.TYPE = "NODE";
				cardRichContent.Html.Body.Elements.Add(cardDoc.DocumentElement);
			}
		}


		
		private static void SerializeMindMapAsync(FreemindMap toReturn, string fileName)
		{
			var serializer = new XmlSerializer(typeof(FreemindMap));

			using (var fs = File.Create(fileName))
			{
				XmlWriterSettings writerSettings = new() { Indent = true, OmitXmlDeclaration = true };
				using var writer = XmlWriter.Create(fs, writerSettings);
				serializer.Serialize(writer, toReturn);
			}


			Logger.LogSuccess($"Mind map {fileName} successfully generated!");
		}
		public async Task ProcessSvgFilesAsync(IList<IMindMapItem> mindMapItems, string fileName,
			AssetConverterConfig webBasedGeneratorConfig, bool enableSvgUpdates, string language)
		{
			string svgFilePath = Path.ChangeExtension(fileName, "svg");
			if (!File.Exists(svgFilePath))
			{
				if (enableSvgUpdates)
				{
					await DisplaySvgFileNotFoundMessage(svgFilePath);
				}
				else
				{
					Logger.LogWarning($"File {svgFilePath} not found and skipped. Automatic conversion failed. Switch \"EnableSVGPrompt\" on for Freemind-assisted SVG generation.");
				}

				if (!File.Exists(svgFilePath))
				{
					return;
				}
			}

			var processedDocs = await ProcessSvgFilesAsync(new[] { svgFilePath });

			foreach (var svgDoc in processedDocs)
			{
				var svgFreemindMap = SVGMaps.FirstOrDefault(s => svgDoc.Key.EndsWith($".{s.DocumentName}", StringComparison.OrdinalIgnoreCase));
				if (svgFreemindMap == null)
				{
					Logger.LogWarning($"No SVGMap matching processed file: {svgDoc.Key}");
					continue;
				}
				await GenerateHtmlSvgWrappers(svgFreemindMap, webBasedGeneratorConfig, svgDoc.Key, () => Task.FromResult(GetSvgContent(svgDoc.Value)), language);
			}

			if (!this.KeepOriginalSVG && File.Exists(svgFilePath))
			{
				File.Delete(svgFilePath);
			}
		}

		internal async Task<Dictionary<string, XDocument>> ProcessSvgFilesAsync(IEnumerable<string> sourceSvgPaths)
		{
			var processedDocs = new Dictionary<string, XDocument>();

			foreach (var svgFilePath in sourceSvgPaths)
			{
				foreach (var svgFreemindMap in SVGMaps)
				{
					var mindMapItems = new List<IMindMapItem>(); // Note: This is a simplification for the test
					var svgSavedFilePath = Path.ChangeExtension(svgFilePath, svgFreemindMap.DocumentName);

					XDocument svgDoc = XDocument.Load(svgFilePath);

					if (!string.IsNullOrEmpty(svgFreemindMap.SvgViewBox))
					{
						svgDoc.Root.SetAttributeValue("viewBox", svgFreemindMap.SvgViewBox);
					}
					if (!string.IsNullOrEmpty(svgFreemindMap.SvgWidth))
					{
						svgDoc.Root.SetAttributeValue("width", svgFreemindMap.SvgWidth);
					}
					if (!string.IsNullOrEmpty(svgFreemindMap.SvgHeight))
					{
						svgDoc.Root.SetAttributeValue("height", svgFreemindMap.SvgHeight);
					}

					XNamespace svgNamespace = "http://www.w3.org/2000/svg";
					XNamespace xlinkNamespace = "http://www.w3.org/1999/xlink";

					UpdateSvgWithItems(svgFreemindMap, mindMapItems, svgDoc, svgNamespace, xlinkNamespace);
					File.WriteAllText(svgSavedFilePath, GetSvgContent(svgDoc), Encoding.UTF8);
					Logger.LogSuccess($"SVG file with detailed content {svgSavedFilePath} successfully saved");
					processedDocs.Add(svgSavedFilePath, svgDoc);
				}
			}
			return processedDocs;
		}

		//private void AdjustSvgViewBox(XDocument svgDoc)
		//{
		//	double minX = double.MaxValue, minY = double.MaxValue;
		//	double maxX = double.MinValue, maxY = double.MinValue;

		//	// Assumer une logique simplifiée pour le traitement des éléments <g> avec translate
		//	var gElements = svgDoc.Descendants().Where(el => el.Name.LocalName == "g");
		//	foreach (var gEl in gElements)
		//	{
		//		var transform = gEl.Attribute("transform")?.Value;
		//		double translateX = 0, translateY = 0;
		//		if (transform != null && transform.StartsWith("translate"))
		//		{
		//			var translateValues = transform.Split('(')[1].Split(')')[0].Split(',');
		//			translateX = double.Parse(translateValues[0]);
		//			translateY = double.Parse(translateValues[1]);
		//		}

		//		// Traiter les éléments enfants comme <text> et <image>
		//		foreach (var child in gEl.Elements())
		//		{
		//			double x = 0, y = 0, width = 0, height = 0;
		//			switch (child.Name.LocalName)
		//			{
		//				case "text":
		//					x = double.Parse(child.Attribute("x")?.Value ?? "0") + translateX;
		//					y = double.Parse(child.Attribute("y")?.Value ?? "0") + translateY;
		//					// Approximation: le texte n'a pas de width/height explicite, ajustement simplifié
		//					minX = Math.Min(minX, x);
		//					minY = Math.Min(minY, y);
		//					break;
		//				case "image":
		//					x = double.Parse(child.Attribute("x")?.Value ?? "0") + translateX;
		//					y = double.Parse(child.Attribute("y")?.Value ?? "0") + translateY;
		//					width = double.Parse(child.Attribute("width")?.Value ?? "0");
		//					height = double.Parse(child.Attribute("height")?.Value ?? "0");
		//					minX = Math.Min(minX, x);
		//					minY = Math.Min(minY, y);
		//					maxX = Math.Max(maxX, x + width);
		//					maxY = Math.Max(maxY, y + height);
		//					break;
		//					// Ajouter des cas pour d'autres types d'éléments ici
		//			}
		//		}
		//	}

		//	// Ajuster les dimensions de la viewBox
		//	string viewBoxValue = $"{minX} {minY} {maxX - minX} {maxY - minY}";
		//	svgDoc.Root.SetAttributeValue("viewBox", viewBoxValue);
		//}



		private static async Task DisplaySvgFileNotFoundMessage(string svgFilePath)
		{
			if (Program.IsInteractive)
			{
				Logger.LogInstructions($"SVG mindmap {svgFilePath} was not found.\n Please download open-source software freemind to generate a SVG export from the original .mm file.\n" +
									   $"[link]https://sourceforge.net/projects/freemind/[/]\nSvg export will be further edited to include fields and links\nPress any key to resume and update or skip the SVG file...");

				await UtilityExtensions.ConsoleKeyPressAsync();
			}
			//await UtilityExtensions.KeyPressSemaphore.WaitAsync();
		}





		private void UpdateSvgWithItems(SVGFreemindMap svgMap, IList<IMindMapItem> items, XDocument svgDoc, XNamespace svgNamespace, XNamespace xlinkNamespace)
		{


			var itemToSVGNodes = CollectPossibleSvgNodes(items, svgDoc, svgNamespace);
			var disambiguatedItemToSVGNode = DisambiguateSvgNodes(itemToSVGNodes, items, svgNamespace);
			var warned = false;
			foreach (var pair in disambiguatedItemToSVGNode)
			{
				UpdateSvgMatch(svgMap, pair.Value, pair.Key, svgNamespace, xlinkNamespace, ref warned);
			}

			// Optionally remove all SVG images
			if (svgMap.RemoveImages)
			{
				switch (this.Format)
				{
					case MindMapFormat.Freemind:
						var imageTags = svgDoc.Descendants(svgNamespace + "image").ToList();
						imageTags = imageTags.Where(i => i.Attributes("width").All(wAttr => wAttr.Value != "60")).ToList();
						imageTags.Remove();
						break;
					case MindMapFormat.Freeplane:
						var iconGroups = svgDoc.Descendants(svgNamespace + "g").Where(g => g.Elements(svgNamespace + "path").Any(x => x.Attributes("stroke").Any(att => att.Value == "none"))).ToList();
						iconGroups.Remove();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				
			}
		}



		private Dictionary<IMindMapItem, List<XElement>> CollectPossibleSvgNodes(IList<IMindMapItem> items, XDocument svgDoc, XNamespace svgNamespace)
		{
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes = new();
			var textGroups = svgDoc.Descendants(svgNamespace + "g").Where(g => g.Elements(svgNamespace + "text").Any()).ToList();

			foreach (var item in items)
			{
				string title = TitleFunc(item);
				var matchingGroups = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title)).ToList();

				if (matchingGroups.Any())
				{
					// Group the g elements by the length of their text content
					var groupedGroups = matchingGroups.GroupBy(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Length);

					// Get the minimum length among the groups
					var groups = groupedGroups as IGrouping<int, XElement>[] ?? groupedGroups.ToArray();
					var minLength = groups.Min(g => g.Key);

					// Retain only the g elements with the minimum length
					var minLengthGroups = groups.First(g => g.Key == minLength).ToList();

					itemToSvgNodes[item] = minLengthGroups;
				}
				else
				{
					var closeMatches = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title.Substring(0, 3))).ToList();
					var closeMatchesMessages = closeMatches.Select(g => string.Join(" ", g.Elements(svgNamespace + "text").Select(t => t.Value))).ToList().Aggregate("", (s1, s2) => $"{s1}\n{s2}");
					Logger.LogProblem($"Could not find Svg node for item {TitleFunc(item)}\nClose matches:\n{closeMatchesMessages}");
				}
			}

			return itemToSvgNodes;
		}


		private Dictionary<IMindMapItem, XElement> DisambiguateSvgNodes(
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes, IList<IMindMapItem> items, XNamespace svgNamespace)
		{
			if (!itemToSvgNodes.Any() || !itemToSvgNodes.First().Value.Any())
			{
				Logger.LogProblem("No SVG nodes to disambiguate.");
				return new Dictionary<IMindMapItem, XElement>();
			}

			var tempNode = itemToSvgNodes.First().Value.First();
			var allNodesList = tempNode.Document.Descendants(svgNamespace + tempNode.Name.LocalName).ToList();
			var nodeIndices = allNodesList.Select((n, i) => new { Node = n, Index = i }).ToDictionary(n => n.Node, n => n.Index);

			foreach (var itemToSvgNode in itemToSvgNodes)
			{
				foreach (var svgNode in itemToSvgNode.Value)
				{
					if (!nodeIndices.ContainsKey(svgNode))
					{
						Logger.LogWarning($"SVG node for item {TitleFunc(itemToSvgNode.Key)} not found in document index. It might be a new or detached node.");
					}
				}
			}

			Dictionary<IMindMapItem, XElement> disambiguatedItemToSvgNode = new();
			Dictionary<XElement, IMindMapItem> svgNodeToItem = new();

			foreach (var pair in itemToSvgNodes)
			{
				IMindMapItem item = pair.Key;
				List<XElement> candidateSvgNodes = pair.Value;

				if (candidateSvgNodes.Count == 1)
				{
					var candidate = candidateSvgNodes.First();
					disambiguatedItemToSvgNode[item] = candidate;
					svgNodeToItem[candidate] = item;
				}
				else
				{
					if (string.IsNullOrEmpty(item.DecimalPath) || item.DecimalPath.Length <= 1)
					{
						Logger.LogProblem($"Cannot determine parent for item {TitleFunc(item)} - {item.Path}");
						continue;
					}
					string parentDecimalPath = item.DecimalPath.Remove(item.DecimalPath.Length - 1);
					var parentItemCandidates = items.Where(f => f.DecimalPath == parentDecimalPath).ToArray();
					if (parentItemCandidates.Length == 0)
					{
						Logger.LogProblem($"Parent item not found for {TitleFunc(item)} - {item.Path}");
						continue;
					}

					var parentItem = parentItemCandidates.First();

					if (!disambiguatedItemToSvgNode.TryGetValue(parentItem, out var parentSvgNode))
					{
						if (itemToSvgNodes.TryGetValue(parentItem, out List<XElement> parentSvgNodes))
						{
							if (parentSvgNodes.Count > 1)
							{
								Logger.LogProblem($"Could not disambiguate SVG nodes for item {TitleFunc(item)} because its parent {TitleFunc(parentItem)} does not have a single corresponding SVG node.");
								continue;
							}
							parentSvgNode = parentSvgNodes.FirstOrDefault();
							if (parentSvgNode == null)
							{
								Logger.LogProblem($"List of parent SVG nodes for {TitleFunc(parentItem)} is empty.");
								continue;
							}
						}
						else
						{
							Logger.LogProblem($"Could not find parent node from {TitleFunc(item)}");
							continue;
						}
					}

					if (!nodeIndices.TryGetValue(parentSvgNode, out int parentIndex))
					{
						Logger.LogProblem($"SVG Node index for parent item: {parentItem.Path}-{TitleFunc(parentItem)} of item {item.Path}-{TitleFunc(item)} not found");
						continue;
					}
					
					var closestSvgNode = candidateSvgNodes
						.Where(node => nodeIndices.ContainsKey(node))
						.OrderBy(node => Math.Abs(nodeIndices[node] - parentIndex))
						.FirstOrDefault();
					
					if (closestSvgNode != null)
					{
						disambiguatedItemToSvgNode[item] = closestSvgNode;
						if (svgNodeToItem.TryGetValue(closestSvgNode, out var existingItem))
						{
							Logger.LogProblem($"Conflicting attribution of SVG node to items: {item.Path}-{TitleFunc(item)} and {existingItem.Path}-{TitleFunc(existingItem)}");
						}
						else
						{
							svgNodeToItem[closestSvgNode] = item;
						}
					}
					else
					{
						Logger.LogWarning($"Could not find a valid matching SVG node for item {TitleFunc(item)} among candidates.");
					}
				}
			}

			return disambiguatedItemToSvgNode;
		}


		private void UpdateSvgMatch(SVGFreemindMap svgMap, XElement match, IMindMapItem item, XNamespace svgNamespace,
			XNamespace xlinkNamespace, ref bool warned)
		{
			if (match.Parent.Name.LocalName == "a" && !warned)
			{
				Logger.LogWarning($"Existing refined content found in SVG file {DocumentName}. Updates will be applied, but some nodes might be missing. Please delete processed SVG file for a fresh processing");
				warned = true;
			}

			string description = DescFunc(item);
			string example = ExampleFunc(item);
			string link = LinkFunc(item);
			string family = FamilleFunc(item);
			string subfamily = SousFamilleFunc(item);
			string subsubfamily = SoussousFamilleFunc(item);


			if (svgMap.SetSVGNodeAttributes)
			{
				match.SetAttributeValue("id", item.Id);
				match.SetAttributeValue("class", "node");
				match.SetAttributeValue(nameof(family), family);
				match.SetAttributeValue(nameof(subfamily), subfamily);
				match.SetAttributeValue(nameof(subsubfamily), subsubfamily);
				match.SetAttributeValue(nameof(description), description);
				match.SetAttributeValue(nameof(example), example);
				match.SetAttributeValue(nameof(link), link);
				match.SetAttributeValue("depth", item.Depth);
				match.SetAttributeValue("familyclass", item.Family.Replace(" ", ""));
			}

			if (svgMap.WrapNodeByLink)
			{
				XElement linkElem = match.Parent.Name.LocalName == "a"
					? match.Parent
					: new XElement(XName.Get("a", svgNamespace.NamespaceName));

				linkElem.SetAttributeValue(XName.Get("href", xlinkNamespace.NamespaceName), link);
				linkElem.SetAttributeValue("target", "_blank");

				if (match.Parent.Name.LocalName != "a")
				{
					match.ReplaceWith(linkElem);
					linkElem.Add(match);
				}
			}


		}


		internal static string GetSvgContent(XDocument svgDoc)
		{
			StringBuilder sb = new();
			XmlWriterSettings settings = new()
			{
				Indent = true,
				IndentChars = "\t", // use tab for indentation
				NewLineChars = Environment.NewLine,
				NewLineHandling = NewLineHandling.Replace
			};

			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				svgDoc.Save(writer);
			}
			string svgContent = sb.ToString();
			return svgContent;
		}



		private static async Task GenerateHtmlSvgWrappers(SVGFreemindMap svgMap, AssetConverterConfig config,
			string svgSavedFilePath,
			Func<Task<string>> svgContent, string language)
		{
			foreach (var htmlSvgWrapper in svgMap.HtmlWrappers)
			{
				var templateFilePath = config.UseDebugParams
					? htmlSvgWrapper.TemplatePathDebug
					: htmlSvgWrapper.TemplatePathRelease;

				string htmlTemplate = (await templateFilePath.GetDocumentPayload()).AsString();

				var languageAwareDocName = htmlSvgWrapper.DocumentName.Replace("[LANGUAGE]", language);

				var htmlFileName = Path.Combine(Directory.GetParent(svgSavedFilePath)!.FullName, languageAwareDocName);  // Path.ChangeExtension(svgSavedFilePath, $".{Path.GetFileName(templateFilePath)}");


				if (File.Exists(htmlFileName) && !config.OverwriteExistingHtmlMaps)
				{
					

					Logger.Log($"Skip existing Html SVG Wrapper: {htmlFileName}");

				}
				else
				{
					var svgRelativePath = svgSavedFilePath.GetRelativePathFrom(Path.GetDirectoryName(htmlFileName));

					// Issue #196: single helper, tested separately (see MindMapHtmlWrapperTests).
					htmlTemplate = MindMapHtmlWrapper.FormatWrapper(htmlTemplate, svgRelativePath, await svgContent());

					File.WriteAllText(htmlFileName, htmlTemplate, Encoding.UTF8);
					Logger.LogSuccess($"Html SVG MindMap wrapper {htmlFileName} successfully saved");
				}

				
			}
		}



		public FallacyMindMapDocumentConfig CloneMindMap()
		{
			// Note: Use MemberwiseClone for shallow copy
			var clone = (FallacyMindMapDocumentConfig)this.MemberwiseClone();

			// Deep copy collections
			clone.Colors = new Dictionary<int, string>(this.Colors);
			clone.FontSizes = new List<int>(this.FontSizes);
			clone.EdgeSizes = new List<int>(this.EdgeSizes);
			clone.SVGMaps = this.SVGMaps.Select(map => (SVGFreemindMap)map.Clone()).ToList();

			return clone;
		}


		protected override DocumentConfig GetClone()
		{
			return CloneMindMap();
		}
	}

	public enum MindMapFormat
	{
		Freemind,
		Freeplane
	}
}