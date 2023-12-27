<template>
  <v-main style="background-color: #EEEEEE;">
    <side-bar/>
    <v-container>
      <v-row>
        <v-col v-for="(card, index) in cards" :key="index" cols="3">
          <v-card>
            <v-card-title class="p-3 pt-2" >
              <v-row align="center" class="mt-1 ml-1 row" :style="{ backgroundColor: card.background }">
                <v-col class="icon-lg mt-n4">
                  <v-avatar class="bg-gradient-dark shadow-dark border-radius-xl">
                    <v-icon color="white">{{ card.icon }}</v-icon>
                  </v-avatar>
                </v-col>
              </v-row>
              <v-row justify="flex-end" class="pt-1">
                <v-col class="text-right">
                  <p class="mb-1 font-weight-regular" style="font-size: 12px; opacity: 0.8">{{ card.title }}</p>
                  <h4 class="mb-0">{{ card.value }}</h4>
                </v-col>
              </v-row>
            </v-card-title>
            <v-card-actions class="p-3">
              <p class="mb-0">
                <span class="text-success text-sm font-weight-bolder">{{ card.percentage }}</span>{{ card.percentageLabel }}
              </p>
            </v-card-actions>
          </v-card>
          <br><br>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script>
export default {
  data() {
    return {
      cards: [
        {
          icon: 'mdi-account',
          title: "Today's Money",
          value: '$53k',
          background: '#111111',
          percentage: '+55% ',
          percentageLabel: 'than last week',
        },
        {
          icon: 'mdi-account',
          title: "Today's Users",
          value: '2,300',
          background: '#FF3399',
          percentage: '+3% ',
          percentageLabel: 'than last month',
        },
        {
          icon: 'mdi-account',
          title: 'New Clients',
          value: '3,462',
          background: '#00FF00',
          percentage: '-2%',
          percentageLabel: 'than yesterday',
        },
        {
          cols: 3,
          icon: 'mdi-account',
          title: 'Sales',
          value: '$103,430',
          background: '#0000FF',
          percentage: '+5% ',
          percentageLabel: 'than yesterday',
        },
      ],
    };
  },
};
</script>

<style scoped>
  .row{
    width: 64px; height: 64px; box-shadow: 0px 0px 8px;border-radius: 12px;
  }
</style>