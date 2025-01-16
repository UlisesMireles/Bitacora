import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Globals } from '../../services/globals';


@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  standalone: false
})
export class MenuComponent implements OnInit {
  usuario:string = "";
  show:boolean = false;
  movilPHistorial = false;
  constructor(private activatedRoute:ActivatedRoute, private route:Router) {
    // $(window).scroll(function(){
    //       var $win = $(window);
    //      // $('.mybtn').css('top',300 -$win.scrollTop());
    //     });
   }

 
  ngOnInit() {
    const params = this.activatedRoute.snapshot.params;
    if(params['usuario']){
      this.usuario=params['usuario'];
      if(params['usuario'] == 'admin'){
        this.show = true;
      }
    }
    if(Globals.pagina==1 && Globals.movil==true){

    }
  }
  get movil(){
    return Globals.movil;
  }

  get pagina(){
    return Globals.pagina;
  }


}
