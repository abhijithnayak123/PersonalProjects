import { Injectable } from '@angular/core';
import { ModuleModel } from '../models/module.model';

@Injectable()
export class ModuleService {
    moduleList: Array<ModuleModel> = [
        new ModuleModel(1, 'Home', '/landing', null),
        new ModuleModel(2, 'Raw Material', '/raw-material', null),
        new ModuleModel(3, 'Manufacturing', '/manufacturing', null),
        new ModuleModel(4, 'Finished Goods', '/finished-goods', null),
        new ModuleModel(5, 'Admin', '/admin', null),
        new ModuleModel(10, 'Inventory', '/raw-material/inventory', 2),
        new ModuleModel(11, 'Planning', '/raw-material/planning', 2),
        new ModuleModel(12, 'Purchasing', '/raw-material/purchasing', 2),
        new ModuleModel(13, 'Container', '/raw-material/container', 2),
        new ModuleModel(14, 'Receiving', '/raw-material/receiving', 2),
        new ModuleModel(15, 'Reports', '/raw-material/reports', 2),
        new ModuleModel(16, 'Planning', '/manufacturing/planning', 3),
        new ModuleModel(17, 'RM Assignment', '/manufacturing/rm-assignment', 3),
        new ModuleModel(18, 'Orders', '/manufacturing/manufacturing-orders', 3),
        new ModuleModel(19, 'Reports', '/manufacturing/reports', 3),
        new ModuleModel(20, 'Inventory', '/finished-goods/inventory', 4),
        new ModuleModel(21, 'Receiving', '/finished-goods/receiving', 4),
        new ModuleModel(22, 'Print Labels', '/finished-goods/print-labels', 4),
        new ModuleModel(23, 'Fulfilment', '/finished-goods/order-fulfilment', 4),
        new ModuleModel(24, 'Reports', '/finished-goods/reports', 4),
        new ModuleModel(25, 'Secure 360', '/admin/secure-360', 5),
        new ModuleModel(26, 'Master', '/admin/master', 5)
      ];
    getHeaderMenu(): Array<ModuleModel> {
        return this.moduleList.filter(c => c.ParentId == null);
    }

    getLeftMenu(parentId: number): Array<ModuleModel> {
        let list = this.moduleList.filter(c => c.ParentId === parentId);
        return list;
	}
}