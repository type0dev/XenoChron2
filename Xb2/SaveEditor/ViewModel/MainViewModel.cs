using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO.Compression;
using System.Linq;
using XbTool;
using XbTool.Bdat;
using XbTool.Save;
using XbTool.Serialization;
using XbTool.Types;

namespace SaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand ReadSaveCommand { get; set; }
        public ICommand WriteSaveCommand { get; set; }
        public ICommand LoadBdatsCommand { get; set; }

        public string SaveFilename { get; set; }
        public string Money { get; set; }
        public ItemDesc[] Acc { get; set; }
        public ItemDesc[] CorCrt { get; set; }
        public ItemDesc[] Cyl { get; set; }
        public ItemDesc[] Key { get; set; }
        public ItemDesc[] Info { get; set; }
        public ItemDesc[] SkRam { get; set; }
        public ItemDesc[] Art { get; set; }
        public ItemDesc[] EnRam { get; set; }
        public ItemDesc[] ElCore { get; set; }
        public ItemDesc[] RCPU { get; set; }
        public ItemDesc[] Boost { get; set; }
        public ItemDesc[] PItem { get; set; }
        public ItemDesc[] ACore { get; set; }
        public ItemDesc[] Trs { get; set; }
        public ItemDesc[] Col { get; set; }
        public ItemDesc[] KItem { get; set; }
        public ItemDesc[] PcWpn { get; set; }

        public SDataSave SaveFile { get; set; }
        public BdatCollection Tables { get; set; }

        public MainViewModel()
        {
            ReadSaveCommand = new RelayCommand(ReadSaveDialog);
            WriteSaveCommand = new RelayCommand(WriteSave);
            LoadBdatsCommand = new RelayCommand(LoadBdats);

            if (IsInDesignModeStatic)
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SaveEditor.bf2savefile.sav.gz"))
                {
                    if (stream == null) return;
                    using (var decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        var file = new byte[0x1176A0];
                        decompressionStream.Read(file, 0, file.Length);
                        SaveFile = new SDataSave(new DataBuffer(file, Game.XB2, 0));
                    }
                }
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SaveEditor.bdats.gz"))
            {
                if (stream == null) return;
                using (var decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    var file = new byte[7394040];
                    decompressionStream.Read(file, 0, file.Length);
                    var bdats = new BdatTables(file, Game.XB2, false);
                    Tables = Deserialize.DeserializeTables(bdats);
                }
            }

            var s = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            s.AddRange(Tables.ITM_PcEquip.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Acc = s.ToArray();

            var t = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            t.AddRange(Tables.ITM_CrystalList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            CorCrt = t.ToArray();

            var u = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            u.AddRange(Tables.ITM_SalvageList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Cyl = u.ToArray();

            var v = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            v.AddRange(Tables.ITM_PreciousList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Key = v.ToArray();

            var w = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            w.AddRange(Tables.ITM_InfoList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Info = w.ToArray();

            var xa = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xa.AddRange(Tables.ITM_HanaAssist.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            SkRam = xa.ToArray();

            var xb = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xb.AddRange(Tables.ITM_HanaNArtsSet.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Art = xb.ToArray();

            var xc = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xc.AddRange(Tables.ITM_HanaArtsEnh.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            EnRam = xc.ToArray();

            var xd = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xd.AddRange(Tables.ITM_HanaAtr.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            ElCore = xd.ToArray();

            var xe = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xe.AddRange(Tables.ITM_HanaRole.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            RCPU = xe.ToArray();

            var xf = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xf.AddRange(Tables.ITM_BoosterList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Boost = xf.ToArray();

            var xg = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xg.AddRange(Tables.ITM_Orb.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            ACore = xg.ToArray();

            var xh = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xh.AddRange(Tables.ITM_FavoriteList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            PItem = xh.ToArray();

            var xi = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xi.AddRange(Tables.ITM_TresureList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Trs = xi.ToArray();

            var xj = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xj.AddRange(Tables.ITM_CollectionList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            Col = xj.ToArray();

            var xl = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xl.AddRange(Tables.ITM_PreciousList.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            KItem = xl.ToArray();

            var xm = new List<ItemDesc> { new ItemDesc { Id = 0, Name = "" } };
            xm.AddRange(Tables.ITM_PcWpnChip.Select(x => new ItemDesc { Id = x.Id, Name = x._Name?.name }));
            PcWpn = xm.ToArray();
        }

        private static string OpenViaFileBrowser(string extension, string filter)
        {
            var openDialog = new OpenFileDialog
            {
                DefaultExt = extension,
                Filter = filter
            };

            if (openDialog.ShowDialog() != true) return null;

            return openDialog.FileName;
        }

        private static string OpenDirViaFileBrowser()
        {
            var openDialog = new CommonOpenFileDialog { IsFolderPicker = true };

            if (openDialog.ShowDialog() != CommonFileDialogResult.Ok) return null;

            return openDialog.FileName;
        }

        public void ReadSave(string filename)
        {
            var file = File.ReadAllBytes(filename);
            var save = new SDataSave(new DataBuffer(file, Game.XB2, 0));
            SaveFile = save;
            SaveFilename = filename;
        }

        public void ReadSaveDialog()
        {
            var filename = OpenViaFileBrowser(".sav", "SAV Files (*.sav)|*.sav|All Files|*.*");
            if (filename == null) return;
            ReadSave(filename);
        }

        public void WriteSave()
        {
            var file = Write.WriteSave(SaveFile);
            File.WriteAllBytes(SaveFilename, file);
        }

        public void LoadBdats()
        {
            var dirName = OpenDirViaFileBrowser();
            var options = new Options
            {
                BdatDir = dirName,
                Game = Game.XB2,
                Filter = "*.bdat"
            };

            BdatCollection tables = Tasks.GetBdatCollection(options);
            Tables = tables;
        }
    }

    public class ItemDesc
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
