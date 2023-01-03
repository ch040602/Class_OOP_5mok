+using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OmokStandAlone;
using System.Net;
using System.IO;

namespace Server
{
    public class NetworkPlayer : Player
    {
        private Thread thread;
        private Socket socket;
        private string status;
        private NetworkPlayer partner;

        public NetworkPlayer(int id, string name, int order, int playerType) : base(id, name, order,playerType)
        {

        }
        public Thread getThread() { return thread; }
        public void setThread(Thread thread) { this.thread = thread; }
        public Socket getSocket() { return this.socket; }
        public void setSocket(Socket socket) { this.socket = socket; }
        public string getStatus() { return this.status; }
        public void setStatus(string status) { this.status = status; }
        public NetworkPlayer getPartner() { return this.partner; }
        public void setPartner(NetworkPlayer partner) { this.partner = partner; }
    }

    public partial class Form1 : Form
    {
        TcpListener listener;
        List<NetworkPlayer> players = new List<NetworkPlayer>();
        bool serverDone = false, serverFull = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread setupThread = new Thread(new ThreadStart(setup));
            setupThread.Start();
        }

        private void setup()
        {
            Socket socket;
            NetworkPlayer player;
            Thread clientHandler;

            listener = new TcpListener(IPAddress.Parse("127.0.0.0"),9050);
            listener.Start();

            do
            {
                socket = listener.AcceptSocket();
                player = readPlayerInfo(socket);
                lock(players)
                {
                    players.Add(player);
                }
                players.Add(player);
                clientHandler = new Thread(new ParameterizedThreadStart(clientHanding));
                player.setThread(clientHandler);
                player.setSocket(socket);
                object args = new Object[1] { player };
                clientHandler.Start(args);
            } while (!serverDone && serverFull);
        }
            private NetworkPlayer readPlayerInfo(Socket socket)
            {
                NetworkStream ns = new NetworkStream(socket);
                BinaryReader reader = new BinaryReader(ns);
                BinaryReader writer = new BinaryReader(ns);
                int id = reader.ReadInt32();
                string name = reader.ReadString();
                NetworkPlayer player = new NetworkPlayer(id, name, 0, 0);

                return player;
            }

            private void clientHanding(object args)
            {
                Array argArray = (Array)args;   
                NetworkPlayer player = (NetworkPlayer) argArray.GetValue(0);

                bool gameDone = false;
                string msg;

                player.setStatus("Ready");  
                while(!gameDone)
                {
                    msg = readMsg(player);
                    Console.WriteLine("server : " + msg + " 수신");
                    gameDone = processingMsg(player, msg);
                }
            }

            private string readMsg(NetworkPlayer player)
            {
                NetworkStream ns = new NetworkStream(player.getSocket());
                BinaryReader reader = new BinaryReader(ns);

                return reader.ReadString();
            }

            private void sendMsg(NetworkPlayer player, string msg)
            {
                NetworkStream ns = new NetworkStream(player.getSocket());
                BinaryReader writer = new BinaryReader(ns);

                writer.Write(msg);
            }

            private bool processingMsg(NetworkPlayer requester, string Msg)
            {
                NetworkPlayer partner;

                if(requester.getStatus().Equals("Ready") && Msg.Equals("WaitingList"))
                {
                    sendWaitingList(requester);
                }
                else if(requester.getStatus().Equals("Ready") && Msg.Equals("addToWaitingList"))
                {
                    requester.setStatus("requester");
                    sendMsg(requester, "addedToWAitingList");
                    sendWaitingList(requester);
                }
                else if((requester.getStatus().Equals("ready") || requester.getStatus().Equals("waiting") && Msg.Equals("matchingRequest"))
                {
                    int id = Int32.Parse(readMsg(requester));

                    if(requester.getId() == id) 
                    {
                        sendMsg(requester, "선택하신 상대가 자신입니다. 다른 상대를 선택해 주세요.");
                    }
                    else
                    {
                        lock(players)
                        {
                            partner = players.Find(x=> x.getId() == id);
                        }
                    
                        sendMsg(partner, "matchingRequested");
                        sendMsg(partner, requester.getId().ToString());
                        sendMsg(partner, requester.getName());
                        partner.setStatus("watchingRequested");
                        partner.setPartner(requester);

                        requester.setStatus("MatchingRequesting");
                        requester.setPartner(partner);
                    }
                }
                else if(requester.getStatus().Equals("MatchingRequested") && Msg.Equals("matchingAccept"))
                {
                    requester.setStatus("Matched");
                    partner = requester.getPartner();
                    sendMsg(partner, "matchingEstabliched");
                    partner.setStatus("Matched");

                    sendMsg(partner, "start1");
                    sendMsg(requester, "start2");
                }
                else if(requester.getStatus().Equals("MatchingRequested") && Msg.Equals("matchingAccept"))
                {
                    requester.setStatus("waiting");
                    partner = requester.getPartner();
                    sendMsg(partner, "matchingNotEstablished");
                    partner.setStatus("Ready");
                }
                else if(requester.getStatus().Equals("Matched") && Msg.Equals("started"))
                {
                    requester.setStatus("Playing");
                }
                else if(requester.getStatus().Equals("Playing") && Msg.Equals("validMove"))
                {
                    string s1 = readMsg(requester);
                    string s2 = readMsg(requester);

                    partner = requester.getPartner();
                    sendMsg(partner, "validMove");
                    sendMsg(partner, s1);
                    sendMsg(partner, s2);
                }
                else if(requester.getStatus().Equals("playing") && Msg.Equals("gameOver"))
                {
                    requester.setStatus("Done");
                    partner = requester.getPartner();
                    sendMsg(partner, " gameOver");
                }
                return false;
            }

            private void sendWaitingList(NetworkPlayer requester)
            {
                int cnt = 0;

                sendMsg(requester, "waitingList");
                int numOfPlayers;
                lock(players)
                {
                    int numOfplayers = players.Count;
                }
                for(int i = 0; i < numOfPlayers; i++)
                {
                    if(players[i] != null && players[i].getStatus().Equals("waiting")) cnt++;
                }
                sendMsg(requester, cnt.ToString());

                int id;
                string name;
                for(int i = 0; i < numOfPlayers; i++)
                {
                    if(players[i] != null && players[i].getStatus().Equals("waiting"))
                    {
                        id = players[i].getId();
                        sendMsg(requester, id.ToString());
                        name = players[i].getName();
                        sendMsg(requester, name);
                    }
                }
             }
           }
        }
    }
}
