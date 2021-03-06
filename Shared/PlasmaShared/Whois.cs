﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZkData
{
    public class Whois
    {
        const string whoisServer = "whois.ripe.net";
        const string sourcesGRS = "ripe-grs,radb-grs,lacnic-grs,jpirr-grs,arin-grs,apnic-grs,afrinic-grs";
        const string sourcesNoGRS = "ripe";

        public async Task<Dictionary<string, string>> QueryByIp(string ip, bool useGRS = false) {
            string sources = "-s " + (useGRS ? sourcesGRS : sourcesNoGRS );
            var data = await QueryWhois(sources + " -l " + ip);
            var result = new Dictionary<string, string>();
            foreach (var line in data.Split('\n').Where(x=>!string.IsNullOrEmpty(x) && x[0] != '%')) {
                var pieces = line.Split(new char[]{':'}, 2);
                var key = pieces.First().Trim();
                var value = pieces.Last().Trim();
                if (!result.ContainsKey(key)) result[key] = value;
            }
            return result;
        }

        public async Task<string> QueryWhois(string command) {
            var tcp = new TcpClient();
            await tcp.ConnectAsync(whoisServer, 43);
            var stream = tcp.GetStream();

            var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteLineAsync(command);
            await streamWriter.FlushAsync();
            return await new StreamReader(stream).ReadToEndAsync();
        }
    }
}
