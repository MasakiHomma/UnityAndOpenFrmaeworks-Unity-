//
//	  UnityOSC - Example of usage for OSC receiver
//
//	  Copyright (c) 2012 Jorge Garcia Martin
//
// 	  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// 	  documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// 	  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// 	  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// 	  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// 	  of the Software.
//
// 	  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// 	  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// 	  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// 	  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// 	  IN THE SOFTWARE.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;

public class OSCControl : MonoBehaviour {

	private Queue queue;
	public string message;

	void Start ()
	{
		queue = new Queue();
		queue = Queue.Synchronized(queue);
		OSCHandler.Instance.Init();
		OSCHandler.Instance.PacketReceivedEvent += OnPacketReceived;
	}

	void OnPacketReceived(OSCServer server, OSCPacket packet) {
		// 来たパケットをキューに積んでおく
		queue.Enqueue(packet);
	}

	void Update()
	{
		while (0 < queue.Count) {
			OSCPacket packet = queue.Dequeue () as OSCPacket;
			if (packet.IsBundle ()) {
				// OSCBundleの場合
				OSCBundle bundle = packet as OSCBundle;
				foreach (OSCMessage msg in bundle.Data) {
					// メッセージの中身にあわせた処理
					message = (string)msg.Data[0];
				}
			} else {
				// OSCMessageの場合はそのまま変換
				OSCMessage msg = packet as OSCMessage;
				// メッセージの中身にあわせた処理
				message = (string)msg.Data[0];
			}
		}
	}
}