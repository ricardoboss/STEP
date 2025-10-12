import * as vscode from "vscode";
import { LanguageClient, LanguageClientOptions, StreamInfo, Trace } from "vscode-languageclient/node";
import * as net from "node:net";

let client: LanguageClient | undefined;

export async function activate(context: vscode.ExtensionContext) {
  const clientOptions: LanguageClientOptions = {
    documentSelector: [{ scheme: "file", language: "STEP", pattern: "*.step" }],
  };

  const serverOptions = () =>
    new Promise<StreamInfo>((resolve, reject) => {
      const socket = net.connect(14246, "127.0.0.1", () => {
        resolve({ writer: socket, reader: socket });
      });
      socket.on("error", reject);
    });

  client = new LanguageClient("step", "STEP LSP Client", serverOptions, clientOptions);

  await client.start();

  context.subscriptions.push({ dispose: () => client?.stop() });
}

export function deactivate(): Thenable<void> | undefined {
	return client?.stop();
}
