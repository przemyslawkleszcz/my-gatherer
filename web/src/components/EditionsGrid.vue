<template>
  <div>
    <div class="md-layout md-gutter">
      <div class="md-layout-item">
        <autocomplete source="https://api.magicthegathering.io/v1/sets?name="
                      ref="autocomplete"
                      placeholder="Search sets"
                      results-property="sets"
                      results-display="name"
                      resultsValue="code"
                      @selected="selected"
                      style="width: 300px; height: 80px;">
        </autocomplete>
        <span>In collection: <span style="color: #7ed875;">{{inInventory}}</span> / {{count}}</span>
      </div>
    </div>

    <md-table>
      <md-table-row>
        <md-table-head></md-table-head>
        <md-table-head>Name</md-table-head>
        <md-table-head>Mana cost</md-table-head>
      </md-table-row>

      <md-table-row v-for="(card, index) in cards" v-bind:class="[card.inInventory == true ? 'inCollection' : '']">
        <md-table-cell>
          #{{index+1}}
        </md-table-cell>
        <md-table-cell v-tooltip.top-center="{content: getCardTooltip(card.imageUrl)}">{{card.name}}</md-table-cell>
        <md-table-cell>
          <mana :manaCost="card.manaCost"></mana>
        </md-table-cell>

      </md-table-row>
    </md-table>
  </div>
</template>

<style>
  .inCollection {
    background-color: #7ed875;
  }

  .autocomplete__box {
    height: 40px !important;
  }
</style>

<script>
  const mtg = require('mtgsdk')
  import axios from 'axios';
  import Mana from '@/components/Mana'
  import Autocomplete from 'vuejs-auto-complete'
  import { getAccessToken } from '../../utils/auth';

  export default {
    name: 'EditionsGrid',
    created() {
      axios.defaults.headers.common['Authorization'] = 'bearer ' + getAccessToken();
    },
    components: {
      Mana,
      Autocomplete
    },
    methods: {
      getCardTooltip(image) {
        return "<img src='" + image + "'/>";
      },
      selected(result) {
        this.cards = [];

        axios.get('http://localhost:58990/api/cards?set=' + result.value)
          .then(response => {
            this.count = response.data.count;
            this.inInventory = response.data.inInventory;

            for (var i = 0; i < response.data.items.length; i++)
              this.cards.push(response.data.items[i]);
          })
          .catch(function (error) {
            console.log(error);
          });
      }
    },
    data() {
      return {
        cards: [],
        count: 0,
        inInventory: 0
      }
    }
  }

</script>
