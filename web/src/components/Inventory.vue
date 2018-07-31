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
    <div>Cards count: {{cardsCount}}</div>
    <div>Distinct count: {{cardsDistinctCount}}</div>
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
    methods: {
      remove(id) {
        axios.delete(process.env.API_URL + '/api/inventory/' + id).then(response => {
          this.getCards();
        }).catch((error) => {
          console.log(error);
        });
      },
      add(id) {
        axios.put(process.env.API_URL + '/api/inventory/' + id).then(response => {
          this.getCards();
        }).catch((error) => {
          console.log(error);
        });
      },
      focus(autocomplete) {
        var input = autocomplete.$el.children[0].children[1].children[0];
        input.focus();
      },
      selected(result) {
        var autocomplete = this.$refs.autocomplete;
        axios.put(process.env.API_URL + '/api/inventory/' + result.value).then(response => {
          this.getCards();
          autocomplete.clearValues();
          this.focus(autocomplete);
        }).catch((error) => {
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
        axios.get(process.env.API_URL + '/api/inventory')
          .then(response => {
            this.cardsCount = response.data.count;
            this.cardsDistinctCount = response.data.distinctCount;
            for (var i = 0; i < response.data.items.length; i++)
              this.cards.push(response.data.items[i]);
          })
          .catch((error) => {
            console.log(error);
          });
      }
    },
    data() {
      return {
        cards: [],
        cardsCount: 0,
        cardsDistinctCount: 0
      }
    }
  }
</script>
