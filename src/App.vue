<template>
  <div class="page-container">
    <md-app>
      <md-app-toolbar class="md-primary">
        <div class="md-toolbar-row">
          <div class="md-toolbar-section-start">
            <span class="md-title">My Title</span>
          </div>
          <div class="md-toolbar-section-end">
            <md-button style="float:right;" v-show="isLoggedIn()" @click="handleLogout()">
              Log out
            </md-button>

            <md-button style="text-align: right;" v-show="!isLoggedIn()" @click="handleLogin()">
              Log In
            </md-button>
          </div>
        </div>
      </md-app-toolbar>
      <md-app-drawer md-permanent="full">
        <md-toolbar class="md-transparent" md-elevation="0">
          MTG Database
        </md-toolbar>
        <md-list>

          <md-list-item>
            <router-link to="/" exact>
              <md-icon>move_to_inbox</md-icon>
              Home
            </router-link>
          </md-list-item>

          <md-list-item>
            <router-link to="/Editions" exact>
              <md-icon>move_to_inbox</md-icon>
              Editions
            </router-link>
          </md-list-item>

          <md-list-item>
            <router-link to="/Database" exact>
              <md-icon>move_to_inbox</md-icon>
              Database
            </router-link>
          </md-list-item>

          <md-toolbar class="md-transparent" md-elevation="0">
            MTG Collection
          </md-toolbar>

          <md-list-item v-if="isLoggedIn()">
            <router-link to="/Inventory" exact>
              <md-icon>move_to_inbox</md-icon>
              Inventory
            </router-link>
          </md-list-item>

        </md-list>
      </md-app-drawer>
      <md-app-content>

        <router-view>
        </router-view>

      </md-app-content>
    </md-app>
  </div>
</template>
<style lang="scss" scoped>

  .md-app {
    border: 1px solid rgba(#000, .12);
  }

  // Demo purposes only
  .md-drawer {
    width: 230px;
    max-width: calc(100vw - 125px);
  }
</style>

<script>
  import { isLoggedIn, login, logout, getAccessToken } from '../utils/auth';

  export default {
    name: 'Normal',
    methods: {
      handleLogin() {
        login();
      },
      handleLogout() {
        logout();
      },
      isLoggedIn() {
        return isLoggedIn();
      },
    }
  }
</script>
