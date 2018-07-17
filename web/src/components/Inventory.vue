<template>
  <div>
    <h3>Inventory</h3>
    <autocomplete source="https://api.magicthegathering.io/v1/cards?name="
                  ref="autocomplete"
                  placeholder="Add cards"
                  results-property="cards"
                  :results-display="formattedDisplay"
                  @selected="selected"
                  style="width: 300px; height: 80px;">
    </autocomplete>
    <span>Cards count: {{cardsCount}}</span>
    <md-table>
      <md-table-row>
        <md-table-head></md-table-head>
        <md-table-head>Quantity</md-table-head>
        <md-table-head>Name</md-table-head>
        <md-table-head>Set name</md-table-head>
        <md-table-head>Mana cost</md-table-head>
      </md-table-row>

      <md-table-row v-for="card in cards">
        <md-table-cell>
          <md-button class="md-icon-button" @click="remove(card.id)">
            <md-icon>remove</md-icon>
          </md-button>
          <md-button class="md-icon-button" @click="add(card.id)">
            <md-icon>add</md-icon>
          </md-button>
        </md-table-cell>
        <md-table-cell>
          {{card.quantity}}
        </md-table-cell>
        <md-table-cell v-tooltip.top-center="{content: getCardTooltip(card.imageUrl)}">{{card.name}}</md-table-cell>
        <md-table-cell>
          {{card.setName}}
        </md-table-cell>
        <md-table-cell>
          <mana :manaCost="card.manaCost"></mana>
        </md-table-cell>

      </md-table-row>
    </md-table>

  </div>
</template>

<style>

  .autocomplete__box {
    height: 40px !important;
  }

  md-tabs.md-no-animation md-tab-content {
    transition: none !important;
  }
</style>

<script>
  import { isLoggedIn, login, logout, getAccessToken } from '../../utils/auth';
  import axios from 'axios';
  const mtg = require('mtgsdk')
  import Mana from '@/components/Mana'
  import Autocomplete from 'vuejs-auto-complete'

  export default {
    name: 'Inventory',
    created() {
      axios.defaults.headers.common['Authorization'] = 'bearer ' + getAccessToken();
      this.getCards();
    },
    components: {
      Mana,
      Autocomplete
    },
    computed: {
      cardsCount() {
        return this.cards.length;
      }
    },
    methods: {
      remove(id) {
        axios.delete('http://localhost:58990/api/cards/' + id).then(response => {
          console.log(response);
          this.getCards();
        }).catch(function (error) {
          console.log(error);
        });
      },
      add(id) {
        axios.put('http://localhost:58990/api/cards/' + id).then(response => {
          console.log(response);
          this.getCards();
        }).catch(function (error) {
          console.log(error);
        });
      },
      selected(result) {
        console.log(result);
        var autocomplete = this.$refs.autocomplete;
        axios.put('http://localhost:58990/api/cards/' + result.value).then(response => {
          console.log(response);
          this.getCards();
          autocomplete.clearValues();
        }).catch(function (error) {
          console.log(error);
        });
      },
      formattedDisplay(result) {
        return result.name + ' [' + result.setName + ']'
      },
      getCardTooltip(image) {
        return "<img src='" + image + "'/>";
      },
      getCards() {
        this.cards = [];
        axios.get('http://localhost:58990/api/cards')
          .then(response => {
            for (var i = 0; i < response.data.length; i++)
              this.cards.push(response.data[i]);
          })
          .catch(function (error) {
            console.log(error);
          });
      }
    },
    data() {
      return {
        cards: []
      }
    }
  }
</script>
