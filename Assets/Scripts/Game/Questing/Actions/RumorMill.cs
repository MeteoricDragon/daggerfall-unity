﻿// Project:         Daggerfall Tools For Unity
// Copyright:       Copyright (C) 2009-2019 Daggerfall Workshop
// Web Site:        http://www.dfworkshop.net
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Source Code:     https://github.com/Interkarma/daggerfall-unity
// Original Author: Gavin Clayton (interkarma@dfworkshop.net)
// Contributors:    Michael Rauter (Nystul)
// 
// Notes:
//

using System.Text.RegularExpressions;
using FullSerializer;

namespace DaggerfallWorkshop.Game.Questing
{
    /// <summary>
    /// add dialog command used in quests to make talk options available.
    /// </summary>
    public class RumorMill : ActionTemplate
    {
        int id;

        public override string Pattern
        {
            get { return @"rumor mill (?<id>\d+)"; }
        }

        public RumorMill(Quest parentQuest)
            : base(parentQuest)
        {
        }

        public override IQuestAction CreateNew(string source, Quest parentQuest)
        {
            // Source must match pattern
            Match match = Test(source);
            if (!match.Success)
                return null;

            // Factory new action
            RumorMill action = new RumorMill(parentQuest);
            action.id = Parser.ParseInt(match.Groups["id"].Value);

            return action;
        }

        public override void Update(Task caller)
        {
            Message message = ParentQuest.GetMessage(id);

            GameManager.Instance.TalkManager.AddQuestRumorToRumorMill(ParentQuest.UID, message);

            SetComplete();
        }

        #region Serialization

        [fsObject("v1")]
        public struct SaveData_v1
        {
            public int id;
        }

        public override object GetSaveData()
        {
            SaveData_v1 data = new SaveData_v1();
            data.id = id;

            return data;
        }

        public override void RestoreSaveData(object dataIn)
        {
            if (dataIn == null)
                return;

            SaveData_v1 data = (SaveData_v1)dataIn;
            id = data.id;
        }

        #endregion
    }
}