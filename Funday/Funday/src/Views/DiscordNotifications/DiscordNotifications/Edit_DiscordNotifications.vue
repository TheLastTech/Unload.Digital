<template>
    <div class="col-12">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
        <p v-if="Message.length >0" class="alert-success">{{Message}}</p>

        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Sold">Sold Web Hook</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="text" id="input-Sold" v-model="TransitObject.Sold" placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Listing">Listing Web Hook</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="text" id="input-Listing" v-model="TransitObject.Listing"
                              placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Error">Error Web Hook </label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="text" id="input-Error" v-model="TransitObject.Error" placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-button block @click="UpdateDiscordNotifications">Update</b-button>
        <p v-if="Success">DiscordNotifications Updated</p> 
    </div>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import {client} from "@/shared";
    import {redirect, Routes} from "@/shared/router";
    import {
        ListOneDiscordNotificationsRequest,
        UpdateDiscordNotificationsRequest
    } from "@/shared/dtos";


    @Component({
        components: {},
    })
    export default class Edit_DiscordNotifications extends Vue {

        Error = "";
        Success = false;
        Id = 0;
        Message = "";

        mounted() {

            this.GetDiscordNotifications();
        }


        async GetDiscordNotifications() {

            this.Error = "";
            const Response = await client.get(new ListOneDiscordNotificationsRequest({}));
            if (!Response.Success) {

                this.Error = Response.Message;
                return;
            }
            //@ts-ignore
            this.TransitObject = Response.DiscordNotificationsItem;

        }

        TransitObject =

            {"Sold": "", "Listing": "", "Error": "", "DiscordNotificationsId": -1};


        async UpdateDiscordNotifications() {
            this.TransitObject.DiscordNotificationsId = this.Id;
            try {
                this.Success = false;
                this.Error = "";
                const Response = await client.put(new UpdateDiscordNotificationsRequest(this.TransitObject));
                if (Response.Success) {
                    this.Success = true;
                } else {
                    this.Error = Response.Message;
                }
            } catch (e) {
                this.Error = e.Message;
            }
        }
    }
</script>

/*

//DiscordNotifications


/*
