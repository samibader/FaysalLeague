﻿import { Component, OnInit, OnDestroy, Inject, ViewEncapsulation, RendererFactory2, PLATFORM_ID } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute, PRIMARY_OUTLET } from '@angular/router';
import { Meta, Title, DOCUMENT, MetaDefinition } from '@angular/platform-browser';
import { Subscription } from 'rxjs/Subscription';
import { isPlatformServer } from '@angular/common';
import { LinkService } from './shared/link.service';

// i18n support
import { TranslateService } from '@ngx-translate/core';
import { REQUEST } from './shared/constants/request';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit, OnDestroy {

    // If no Title is provided, we'll use a default one before the dash(-)
    private defaultPageTitle: string = 'Alfaysal League 10/10';

    private routerSub$: Subscription;

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private title: Title,
        private meta: Meta,
        private linkService: LinkService,
        public translate: TranslateService,
        @Inject(REQUEST) private request
    ) {
        // this language will be used as a fallback when a translation isn't found in the current language
        translate.setDefaultLang('en');

        // the lang to use, if the lang isn't available, it will use the current loader to get them
        translate.use('en');
    }

    ngOnInit() {
        // Change "Title" on every navigationEnd event
        // Titles come from the data.title property on all Routes (see app.routes.ts)
        this._changeTitleOnNavigation();
    }

    ngOnDestroy() {
        // Subscription clean-up
        this.routerSub$.unsubscribe();
    }

    private _changeTitleOnNavigation() {

        this.routerSub$ = this.router.events
            .filter(event => event instanceof NavigationEnd)
            .map(() => this.activatedRoute)
            .map<ActivatedRoute, ActivatedRoute>((route) => {
                while (route.firstChild) {
                    route = route.firstChild;
                }
                return route;
            })
            .filter<ActivatedRoute>(route => route.outlet === 'primary')
            .mergeMap(route => route.data)
            .subscribe((event) => {
                this._setMetaAndLinks(event);
            });
    }

    private _setMetaAndLinks(event) {

        // Set Title if available, otherwise leave the default Title
        const title = event['title']
            ? `${event['title']}`
            : `${this.defaultPageTitle}`;

        this.title.setTitle(title);

        const metaData:MetaDefinition[] = event['meta'] || [];
        const linksData:MetaDefinition[] = event['links'] || [];

        for (let i = 0; i < metaData.length; i++) {
            this.meta.updateTag(metaData[i]);
        }

        for (let i = 0; i < linksData.length; i++) {
            this.linkService.addTag(linksData[i]);
        }
    }

}

