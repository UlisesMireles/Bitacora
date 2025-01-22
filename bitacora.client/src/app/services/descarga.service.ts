import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { saveAs } from 'file-saver';
@Injectable({
  providedIn: 'root'
})
export class DescargaService {
  constructor(private http:HttpClient, @Inject("BASE_URL") private baseUrl:string ) { }

  downloadFile(archivo:any):Observable<Blob>{
    var datos= {fileName:archivo};
    const requestOptions: Object = {
      responseType: 'blob',
      params: datos
    }
    //console.log(this.baseUrl)
    return this.http.get<Blob>(this.baseUrl+"api/Reportes/descarga/"+archivo,requestOptions).pipe(map(
      (res) => {
          //console.log(res)
            //var blob = new Blob([res.blob()], {type: "application/vnd.ms-excel"} )
            const blob = new Blob([res], { type : 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            
            //console.log("sdsdds")
            //console.log(blob)   
            return blob;      
      },
      (err:any)=>{//console.log(err)
      }));
  }

  downloadFile2(filePath: string): Observable<any>{
    let fileExtension = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    let input = filePath;
    var x = this.http.post(this.baseUrl+"api/Reportes/DownloadFile/"+input, '',
    { responseType: 'blob' }).pipe(
    map(
      (res) => {
            const blob = new Blob([res], { type : 'application/vnd.ms.excel' });
            const file = new File([blob], filePath + '.xlsx', { type: 'application/vnd.ms.excel' });
            saveAs(file);
            return blob;            
      })
    )
      return x;
  }

  
}
