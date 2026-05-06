<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { notificationsApi } from "../api/notificationsApi";
import { signalRService } from "../services/signalRService";
import type { Notification } from "../types/notification";

const notifications = ref<Notification[]>([]);
const connectionStatus = ref("Disconnected");

const form = ref({
  title: "",
  message: "",
  type: "Info",
});

const unreadCount = computed(() =>
  notifications.value.filter((x) => !x.isRead).length
);

const loadNotifications = async () => {
  notifications.value = await notificationsApi.getAll();
};

const createNotification = async () => {
  if (!form.value.title || !form.value.message) return;

  await notificationsApi.create(form.value);

  form.value = {
    title: "",
    message: "",
    type: "Info",
  };
};

const selectedFilter = ref("All");

const filteredNotifications = computed(() => {
  if (selectedFilter.value === "Unread") {
    return notifications.value.filter((x) => !x.isRead);
  }

  if (selectedFilter.value === "Read") {
    return notifications.value.filter((x) => x.isRead);
  }

  return notifications.value;
});

const successCount = computed(() =>
  notifications.value.filter((x) => x.type === "Success").length
);

const warningCount = computed(() =>
  notifications.value.filter((x) => x.type === "Warning").length
);

const errorCount = computed(() =>
  notifications.value.filter((x) => x.type === "Error").length
);

const markAsRead = async (id: number) => {
  await notificationsApi.markAsRead(id);

  notifications.value = notifications.value.map((item) =>
    item.notificationId === id ? { ...item, isRead: true } : item
  );
};

onMounted(async () => {
  await loadNotifications();

  await signalRService.start();
  connectionStatus.value = signalRService.getState();

  signalRService.onNotificationCreated((notification) => {
    notifications.value = [notification, ...notifications.value];
  });

  signalRService.onNotificationUpdated((id) => {
    notifications.value = notifications.value.map((item) =>
      item.notificationId === id ? { ...item, isRead: true } : item
    );
  });
});
</script>

<template>
  <main class="page">
    <section class="hero">
      <div>
        <p class="eyebrow">ServicePulse RealTime</p>
        <h1>Real-Time Service Notifications</h1>
        <p class="subtitle">
          ASP.NET Core, SignalR, SQL Server and Vue working together.
        </p>
      </div>

      <div class="status-card">
        <span>SignalR</span>
        <strong>{{ connectionStatus }}</strong>
        <small>{{ unreadCount }} unread notifications</small>
      </div>
    </section>

    <section class="stats">
        <div class="stat-card">
            <span>Total</span>
            <strong>{{ notifications.length }}</strong>
        </div>

        <div class="stat-card">
            <span>Unread</span>
            <strong>{{ unreadCount }}</strong>
        </div>

        <div class="stat-card">
            <span>Success</span>
            <strong>{{ successCount }}</strong>
        </div>

        <div class="stat-card">
            <span>Warnings</span>
            <strong>{{ warningCount }}</strong>
        </div>

        <div class="stat-card">
            <span>Errors</span>
            <strong>{{ errorCount }}</strong>
        </div>
    </section>

    <section class="grid">
      <form class="card form-card" @submit.prevent="createNotification">
        <h2>Create Notification</h2>

        <input v-model="form.title" placeholder="Title" />

        <textarea v-model="form.message" placeholder="Message"></textarea>

        <select v-model="form.type">
          <option>Info</option>
          <option>Success</option>
          <option>Warning</option>
          <option>Error</option>
        </select>

        <button type="submit">Send Notification</button>
      </form>

      <section class="card">
        <div class="list-header">
          <h2>Live Feed</h2>
          <span>{{ notifications.length }} total</span>
          <select v-model="selectedFilter" class="filter-select">
            <option>All</option>
            <option>Unread</option>
            <option>Read</option>
         </select>
        </div>

        <div v-if="notifications.length === 0" class="empty">
          No notifications yet.
        </div>

        <article
          v-for="item in filteredNotifications"
          :key="item.notificationId"
          class="notification"
          :class="{ unread: !item.isRead }"
        >
          <div>
            <div class="notification-top">
              <strong>{{ item.title }}</strong>
              <span>{{ item.type }}</span>
            </div>

            <p>{{ item.message }}</p>
            <p>{{ item.message }}</p>

            <small>{{ new Date(item.createdAt).toLocaleString() }}</small>
          </div>

          <button
            v-if="!item.isRead"
            class="secondary"
            @click="markAsRead(item.notificationId)"
          >
            Mark as read
          </button>
        </article>
      </section>
    </section>
  </main>
</template>