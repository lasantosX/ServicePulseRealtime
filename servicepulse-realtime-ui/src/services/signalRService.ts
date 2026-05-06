import * as signalR from "@microsoft/signalr";
import type { Notification } from "../types/notification";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5245/hubs/notifications")
  .withAutomaticReconnect()
  .build();

export const signalRService = {
  start: async (): Promise<void> => {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
      await connection.start();
    }
  },

  onNotificationCreated: (callback: (notification: Notification) => void): void => {
    connection.on("NotificationCreated", callback);
  },

  onNotificationUpdated: (callback: (id: number) => void): void => {
    connection.on("NotificationUpdated", callback);
  },

  getState: (): string => connection.state,
};