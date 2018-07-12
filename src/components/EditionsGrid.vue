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
        <span>In collection: <span style="color: #7ed875;">{{cardsInCollection}}</span> / {{cardsCount}}</span>
      </div>
    </div>

    <md-table>
      <md-table-row>
        <md-table-head>Name</md-table-head>
        <md-table-head>Mana cost</md-table-head>
      </md-table-row>

      <md-table-row v-for="card in cards" v-bind:class="[card.inCollection == true ? 'inCollection' : '']">
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
      this.getInventoryCards();
    },
    components: {
      Mana,
      Autocomplete
    },
    computed: {
      cardsInCollection() {
        return this.cards.filter((item) => {
          return this.inventory.includes(item.id);
        }).length;
      },
      cardsCount() {
        return this.cards.length;
      }
    },
    methods: {
      getCardTooltip(image) {
        return "<img src='" + image + "'/>";
      },
      selected(result) {
        this.cards = [];

        mtg.card.all({ set: result.value })
          .on("data", card => {
            this.cards.push({
              id: card.id,
              name: card.name,
              manaCost: card.manaCost,
              inCollection: this.inventory.includes(card.id),
              imageUrl: card.imageUrl == null
                ? ""
                : card.imageUrl
            });
          });
      },
      getInventoryCards() {
        this.inventory = [];
        axios.get('http://localhost:58990/api/cards')
          .then(response => {
            for (var i = 0; i < response.data.length; i++)
              this.inventory.push(response.data[i].id)
          })
          .catch(function (error) {
            console.log(error);
          });
      }
    },
    data() {
      return {
        cards: [],
        inventory: []
      }
    }
  }

</script>
