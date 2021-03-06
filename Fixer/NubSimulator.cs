﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LobbyClient;
using ZkData;
using ZkData.UnitSyncLib;

namespace Fixer
{
    class NubSimulator
    {
        public void RunNub(int num)
        {
            var tas = new TasClient(null, "Nubotron", GlobalConst.ZkLobbyUserCpu);
            var name = "TestNub" + num;
            var ord = num / 16;
            var batname = "Test " + ord;

            tas.Connected += (sender, args) => { tas.Login(name, "dummy"); };
            tas.LoginDenied += (sender, args) => { tas.Register(name, "dummy"); };
            tas.AgreementRecieved += (sender, recieved) => {
                tas.AcceptAgreement();
                tas.Login(name, "dummy");
            };

            

            tas.UserAdded += (sender, args) => {
                if (args.Data.Name == name) {
                    tas.JoinChannel("bots");
                    if (num%16 == 0)
                        tas.OpenBattle(new Battle("91.0", null, 4955, 16, 0, new Map("SmallDivide"), "Test " + ord,
                            new Mod() { Name = "Zero-K v1.3.1.15" }, new BattleDetails()));
                    else {
                        var bat = tas.ExistingBattles.Values.FirstOrDefault(x => x.Title == batname);
                        if (bat != null) tas.JoinBattle(bat.BattleID);
                    }
                }
            };
            tas.BattleFound += (sender, args) => {
                if (args.Data.Title == batname) {
                    tas.JoinBattle(args.Data.BattleID);
                }
            };
            
            
            
            tas.Connect(GlobalConst.LobbyServerHost, GlobalConst.LobbyServerPort);

            
            while (true) {
                Thread.Sleep(5000);
            }
        }

        public void SpawnMany()
        {
            ThreadPool.SetMaxThreads(500, 500);
            for (int i = 0; i < 100; i++) {
                int i1 = i;
                //Thread.Sleep(100);
                Task.Factory.StartNew(() => { RunNub(i1); });
            }
        }


    }
}
