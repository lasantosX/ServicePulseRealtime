import axios from "axios";
import type { CreateNotificationRequest, Notification } from "../types/notification";

const api = axios.create({
  baseURL: "http://localhost:5245/api",
});

export const notificationsApi = {
  getAll: async (): Promise<Notification[]> => {
    const response = await api.get<Notification[]>("/Notifications");
    return response.data;
  },

  create: async (request: CreateNotificationRequest): Promise<Notification> => {
    const response = await api.post<Notification>("/Notifications", request);
    return response.data;
  },

  markAsRead: async (id: number): Promise<void> => {
    await api.put(`/Notifications/${id}/mark-as-read`);
  },
};