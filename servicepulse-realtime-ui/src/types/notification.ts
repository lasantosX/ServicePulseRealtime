export interface Notification {
  notificationId: number;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAt: string;
}

export interface CreateNotificationRequest {
  title: string;
  message: string;
  type: string;
}