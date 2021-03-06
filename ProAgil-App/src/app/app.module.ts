import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { AppRoutingModule } from "./app-routing.module";
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { ModalModule } from "ngx-bootstrap/modal";

import { AppComponent } from "./app.component";
import { EventosComponent } from "./eventos/eventos.component";
import { NavComponent } from "./nav/nav.component";

import { DateTimeFormatPipePipe } from "./_helps/DateTimeFormatPipe.pipe";

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    NavComponent,
    DateTimeFormatPipePipe,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
