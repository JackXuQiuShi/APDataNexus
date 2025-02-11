import { Component, Input, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
    selector: 'app-confirmation',
    templateUrl: './confirmation.component.html',
    styleUrls: ['./confirmation.component.scss'],
    standalone: false
})
export class ConfirmationComponent  implements OnInit {

  @Input() message!: string; // 添加这个属性以接收确认消息
  
  constructor(private modalController: ModalController) { }

  ngOnInit() {}

  confirm() {
    // Handle the confirmation logic here
    // You can emit an event or perform any necessary actions
    this.modalController.dismiss(true); // Dismiss the modal with a confirmation flag
  }

  dismiss() {
    // Dismiss the modal without confirmation
    this.modalController.dismiss(false);
  }
}
